# User Guide — Lead Route Optimizer

## Who this is for
This guide is written for **Inside Sales Representatives** who receive weekly lead lists from their manager and may also maintain their own personal lead lists.

The goal of the Lead Route Optimizer is to help you **decide the order in which to visit your leads** so that your total travel distance is minimized.

---

## What you need before starting

To successfully use the application, you will need:

- One or two CSV files:
  - A **manager-provided lead list** (required)
  - A **personal lead list** (optional)
- Each lead **must include latitude and longitude** values
- Your **home latitude and longitude** (starting point for the route)

> Tip: Get latitude and longitude from Google Maps by right-clicking on a location and copying the coordinates.

---

## Step-by-step usage

### 1. Upload your lead lists

1. Open the application
2. Upload the CSV file provided by your manager
   - Select `Manager` as the source type
3. (Optional) Upload your personal CSV file
   - Select `Personal` as the source type

After each upload, the system will show:
- Total number of rows processed
- Number of valid leads
- Number of invalid leads

Invalid rows are ignored but kept for reference.

---

### 2. Enter your starting location

Enter your **home latitude and longitude**.

This location is used as the starting point for route planning.

---

### 3. Generate your route plan

Once your lead lists and home location are entered:

1. Generate the route plan
2. The system combines all valid leads
3. Duplicate leads are automatically removed
4. Leads are ordered to minimize total travel distance

---

### 4. Review your route

The resulting route shows:

- The order in which to visit leads
- Distance between each stop
- Total estimated distance for the route

This list represents the recommended visit order for your day or week.

---

## CSV file format

### Required columns

Every CSV file **must include**:

- `LeadName`
- `Latitude`
- `Longitude`

### Optional columns

These are optional but recommended for clarity:

- `Street`
- `City`
- `State`
- `Zip`

### Example CSV

```csv
LeadName,Latitude,Longitude,Street,City,State,Zip
Lead A,41.8781,-87.6298,1 Wacker Dr,Chicago,IL,60601
Lead B,41.8810,-87.6237,200 E Randolph St,Chicago,IL,60602
```

---

