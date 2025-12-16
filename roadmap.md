# Roadmap

This roadmap outlines how the Lead Route Optimizer would evolve with additional time from my POV.

---

## If I had 2 more days

**Goal: polish and usability improvements**

- Add a minimal React UI:
  - Upload manager and personal CSV files
  - Enter home latitude/longitude
  - Generate and view route results in a table
- Improve CSV validation feedback in the UI (show invalid rows and reasons)
- Add basic client-side validation (required fields, numeric ranges)
- Improve API error messages and consistency
- Add pagination or limits for very large CSV uploads

---

## If I had 2 more weeks

**Goal: make the app ready for production**

- Replace straight line distance with driving distance using a mapping API
- Add distance caching to reduce external API calls
- Introduce authentication and user accounts
- Persist and retrieve saved plans per user
- Add a map visualization of the route
- Improve routing algorithm (this probably will take me long because I will need to research)
- Add structured logging and basic monitoring, probably will also add rate limiting.
- Add more test coverage

---

## If I had 4 more weeks

**Goal: scale for wide usage**

- Add role based access
- Allow managers to upload and assign lead lists to ISRs
- Add a module for reports
- Support multiple starting locations
- Improve deduplication logic across historical uploads
- Introduce background processing for large calculations
- Add API versioning

---

## If I had 8 more weeks

**Goal: optimize, automate, and expand capabilities**

- Advanced route optimization
- Add a mobile or PWA app for users (this will take more time)
- Integration with CRM if any
- Historical analytics and trend analysis
- Cost modeling based on fuel, vehicle type, time spent, etc
- Improve security

---