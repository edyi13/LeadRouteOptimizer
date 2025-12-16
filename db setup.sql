IF DB_ID(N'LeadRoutePlanner') IS NULL
BEGIN
    CREATE DATABASE LeadRoutePlanner;
END
GO

USE LeadRoutePlanner;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'app')
BEGIN
    EXEC('CREATE SCHEMA app AUTHORIZATION dbo;');
END
GO

-- UploadBatch
IF OBJECT_ID(N'app.UploadBatch', N'U') IS NULL
BEGIN
    CREATE TABLE app.UploadBatch
    (
        Id UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT PK_UploadBatch PRIMARY KEY
            CONSTRAINT DF_UploadBatch_Id DEFAULT NEWID(),

        SourceType NVARCHAR(20) NOT NULL, -- 'Manager' | 'Personal'
        OriginalFileName NVARCHAR(260) NULL,

        CreatedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_UploadBatch_CreatedAtUtc DEFAULT SYSUTCDATETIME(),

        CreatedBy NVARCHAR(100) NULL
    );

    ALTER TABLE app.UploadBatch
      ADD CONSTRAINT CK_UploadBatch_SourceType
      CHECK (SourceType IN (N'Manager', N'Personal'));
END
GO

-- Lead
IF OBJECT_ID(N'app.Lead', N'U') IS NULL
BEGIN
    CREATE TABLE app.Lead
    (
        Id UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT PK_Lead PRIMARY KEY
            CONSTRAINT DF_Lead_Id DEFAULT NEWID(),

        UploadBatchId UNIQUEIDENTIFIER NOT NULL,

        LeadName NVARCHAR(200) NOT NULL,

        Street NVARCHAR(200) NULL,
        City   NVARCHAR(100) NULL,
        State  NVARCHAR(50) NULL,
        Zip    NVARCHAR(20) NULL,

        Latitude  DECIMAL(9,6) NOT NULL,
        Longitude DECIMAL(9,6) NOT NULL,

        NormalizedKey AS
          (
            LOWER(LTRIM(RTRIM(LeadName))) + N'|' +
            CONVERT(NVARCHAR(50), ROUND(Latitude, 5)) + N'|' +
            CONVERT(NVARCHAR(50), ROUND(Longitude, 5))
          ) PERSISTED,

        Status NVARCHAR(20) NOT NULL, -- 'Valid' | 'Invalid'
        ErrorMessage NVARCHAR(1000) NULL,

        RawRowJson NVARCHAR(MAX) NULL,

        CreatedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_Lead_CreatedAtUtc DEFAULT SYSUTCDATETIME()
    );

    ALTER TABLE app.Lead
      ADD CONSTRAINT FK_Lead_UploadBatch
      FOREIGN KEY (UploadBatchId) REFERENCES app.UploadBatch(Id);

    ALTER TABLE app.Lead
      ADD CONSTRAINT CK_Lead_Status
      CHECK (Status IN (N'Valid', N'Invalid'));

    ALTER TABLE app.Lead
      ADD CONSTRAINT CK_Lead_Latitude
      CHECK (Latitude BETWEEN -90.0 AND 90.0);

    ALTER TABLE app.Lead
      ADD CONSTRAINT CK_Lead_Longitude
      CHECK (Longitude BETWEEN -180.0 AND 180.0);
END
GO

-- RoutePlan
IF OBJECT_ID(N'app.RoutePlan', N'U') IS NULL
BEGIN
    CREATE TABLE app.RoutePlan
    (
        Id UNIQUEIDENTIFIER NOT NULL
            CONSTRAINT PK_RoutePlan PRIMARY KEY
            CONSTRAINT DF_RoutePlan_Id DEFAULT NEWID(),

        HomeLatitude  DECIMAL(9,6) NOT NULL,
        HomeLongitude DECIMAL(9,6) NOT NULL,

        TotalDistanceKm DECIMAL(12,3) NOT NULL
            CONSTRAINT DF_RoutePlan_TotalDistanceKm DEFAULT (0),

        CreatedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_RoutePlan_CreatedAtUtc DEFAULT SYSUTCDATETIME()
    );

    ALTER TABLE app.RoutePlan
      ADD CONSTRAINT CK_RoutePlan_Latitude
      CHECK (HomeLatitude BETWEEN -90.0 AND 90.0);

    ALTER TABLE app.RoutePlan
      ADD CONSTRAINT CK_RoutePlan_Longitude
      CHECK (HomeLongitude BETWEEN -180.0 AND 180.0);

    ALTER TABLE app.RoutePlan
      ADD CONSTRAINT CK_RoutePlan_TotalDistanceKm
      CHECK (TotalDistanceKm >= 0);
END
GO

-- RouteStop
IF OBJECT_ID(N'app.RouteStop', N'U') IS NULL
BEGIN
    CREATE TABLE app.RouteStop
    (
        RoutePlanId UNIQUEIDENTIFIER NOT NULL,
        Sequence INT NOT NULL, -- 1..N
        LeadId UNIQUEIDENTIFIER NOT NULL,

        LegDistanceKm DECIMAL(12,3) NOT NULL
            CONSTRAINT DF_RouteStop_LegDistanceKm DEFAULT (0),

        CreatedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_RouteStop_CreatedAtUtc DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_RouteStop PRIMARY KEY (RoutePlanId, Sequence)
    );

    ALTER TABLE app.RouteStop
      ADD CONSTRAINT FK_RouteStop_RoutePlan
      FOREIGN KEY (RoutePlanId) REFERENCES app.RoutePlan(Id)
      ON DELETE CASCADE;

    ALTER TABLE app.RouteStop
      ADD CONSTRAINT FK_RouteStop_Lead
      FOREIGN KEY (LeadId) REFERENCES app.Lead(Id);

    ALTER TABLE app.RouteStop
      ADD CONSTRAINT CK_RouteStop_Sequence
      CHECK (Sequence > 0);

    ALTER TABLE app.RouteStop
      ADD CONSTRAINT CK_RouteStop_LegDistanceKm
      CHECK (LegDistanceKm >= 0);
END
GO

-- RoutePlanUpload (join)
IF OBJECT_ID(N'app.RoutePlanUpload', N'U') IS NULL
BEGIN
    CREATE TABLE app.RoutePlanUpload
    (
        RoutePlanId UNIQUEIDENTIFIER NOT NULL,
        UploadBatchId UNIQUEIDENTIFIER NOT NULL,

        LinkedAtUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_RoutePlanUpload_LinkedAtUtc DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_RoutePlanUpload PRIMARY KEY (RoutePlanId, UploadBatchId)
    );

    ALTER TABLE app.RoutePlanUpload
      ADD CONSTRAINT FK_RoutePlanUpload_RoutePlan
      FOREIGN KEY (RoutePlanId) REFERENCES app.RoutePlan(Id)
      ON DELETE CASCADE;

    ALTER TABLE app.RoutePlanUpload
      ADD CONSTRAINT FK_RoutePlanUpload_UploadBatch
      FOREIGN KEY (UploadBatchId) REFERENCES app.UploadBatch(Id);
END
GO

PRINT 'DB setup complete (tables only, EF-friendly).';
GO
