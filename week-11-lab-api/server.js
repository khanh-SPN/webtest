// FIT2095 Week 11 lab assessment: the provided backend.
// The REST routes below are complete and you do not need to change them.
// Your task sheet asks you to add real-time events to this file.
//
//   npm install
//   npm start
//
// Data lives in memory, so restarting the server puts everything back.
// Every path starts with your own 8-digit Student ID, then your scenario:
//
//   http://localhost:8080/<StudentID>/<scenario>/...

import express from "express";
import { createServer } from "node:http";
import { Server } from "socket.io";

const app = express();
app.use(express.json());

// Socket.IO shares one HTTP server (and port 8080) with the REST API.
const httpServer = createServer(app);
const io = new Server(httpServer, { cors: { origin: "*" } });

const PORT = 8080;

// The Angular dev server (port 4200) is a different origin from this API.
// This covers the REST routes only.
app.use((req, res, next) => {
  res.set("Access-Control-Allow-Origin", "*");
  res.set("Access-Control-Allow-Headers", "Content-Type");
  res.set("Access-Control-Allow-Methods", "GET, POST, DELETE, OPTIONS");
  if (req.method === "OPTIONS") return res.sendStatus(204);
  next();
});

// -----------------------------------------------------------------------------
// Scenarios: each has one collection. `fields` are required when adding a record.
// -----------------------------------------------------------------------------

const scenarios = {
  orchard: {
    name: "harvests",
    fields: ["variety", "harvestedBy", "harvestDate", "notes"],
    items: [
      { id: "h1", variety: "Bramley Apple", harvestedBy: "Farah Nkemelu", harvestDate: "2026-03-14", notes: "Picked from the north row before the rain. Two crates went to the food bank and the rest to the cider press." },
      { id: "h2", variety: "Victoria Plum", harvestedBy: "Declan Osei", harvestDate: "2026-03-10", notes: "Fruit bruises easily, so everything was picked by hand into shallow trays and stored in the cool shed overnight." },
      { id: "h3", variety: "Conference Pear", harvestedBy: "Mei Tanaka", harvestDate: "2026-03-21", notes: "The oldest tree in the orchard gave its best crop in years. Some windfalls were left on the ground for the birds." },
    ],
  },
  sailing: {
    name: "outings",
    fields: ["boatName", "skipper", "outingDate", "notes"],
    items: [
      { id: "o1", boatName: "Kittiwake", skipper: "Farah Nkemelu", outingDate: "2026-03-14", notes: "Light southerly breeze all morning. Practised tacking around the outer marker and came back in before the tide turned." },
      { id: "o2", boatName: "Sandpiper", skipper: "Declan Osei", outingDate: "2026-03-10", notes: "Training sail with a new member as crew. The jib sheet jammed twice, so the cleat needs replacing before the next outing." },
      { id: "o3", boatName: "Cormorant", skipper: "Mei Tanaka", outingDate: "2026-03-21", notes: "Strong gusts past the breakwater, so both hulls were reefed early. Returned with a small tear in the trampoline." },
    ],
  },
  boardgames: {
    name: "bookings",
    fields: ["gameTitle", "memberName", "bookingDate", "notes"],
    items: [
      { id: "k1", gameTitle: "Harbour Traders", memberName: "Farah Nkemelu", bookingDate: "2026-03-14", notes: "Table by the window for four players. One of the group is new to the game and asked for a volunteer to explain the rules." },
      { id: "k2", gameTitle: "Lantern Path", memberName: "Declan Osei", bookingDate: "2026-03-10", notes: "A family booking with two children under ten. They would like the low table near the counter and an early finish." },
      { id: "k3", gameTitle: "Deep Vault", memberName: "Mei Tanaka", bookingDate: "2026-03-21", notes: "Second attempt at the co-operative campaign. Please keep the box set up from last week if nobody else has asked for it." },
    ],
  },
  kiln: {
    name: "firings",
    fields: ["glazeName", "firedBy", "firingDate", "notes"],
    items: [
      { id: "f1", glazeName: "Celadon Mist", firedBy: "Farah Nkemelu", firingDate: "2026-03-14", notes: "Twelve bowls on the top two shelves. The glaze pooled nicely in the carved lines, but one piece cracked while cooling." },
      { id: "f2", glazeName: "Tenmoku", firedBy: "Declan Osei", firingDate: "2026-03-10", notes: "Fired slightly hotter than planned. The rims broke to rust as hoped, although two mugs stuck to the kiln shelf." },
      { id: "f3", glazeName: "Oatmeal Matte", firedBy: "Mei Tanaka", firingDate: "2026-03-21", notes: "A full load of dinner plates, glazed thinly so they stay food safe. Every piece came out even and none had pinholes." },
    ],
  },
  wildlife: {
    name: "intakes",
    fields: ["commonName", "rescuedBy", "intakeDate", "notes"],
    items: [
      { id: "i1", commonName: "Ringtail Possum", rescuedBy: "Farah Nkemelu", intakeDate: "2026-03-14", notes: "Found on a suburban footpath after a storm. Alert and feeding well, and is being kept in a darkened enclosure by day." },
      { id: "i2", commonName: "Tawny Frogmouth", rescuedBy: "Declan Osei", intakeDate: "2026-03-10", notes: "Brought in with a suspected wing injury. Stresses easily, so handling is limited to one short check each morning." },
      { id: "i3", commonName: "Blue-tongue Lizard", rescuedBy: "Mei Tanaka", intakeDate: "2026-03-21", notes: "Rescued from a backyard pool filter. Slow at first, but now basking normally and should be released within the week." },
    ],
  },
  radio: {
    name: "slots",
    fields: ["showTitle", "host", "airDate", "notes"],
    items: [
      { id: "a1", showTitle: "Morning Tide", host: "Farah Nkemelu", airDate: "2026-03-14", notes: "Local news and two studio interviews. The second guest is joining by phone, so the line needs testing before going on air." },
      { id: "a2", showTitle: "Vinyl Hour", host: "Declan Osei", airDate: "2026-03-10", notes: "Records from the station archive, all pressed before 1980. Turntable two has a worn stylus and should not be used." },
      { id: "a3", showTitle: "Garden Line", host: "Mei Tanaka", airDate: "2026-03-21", notes: "Listeners call in with plant questions for the whole hour. A volunteer is needed to screen calls on the second line." },
    ],
  },
};

// -----------------------------------------------------------------------------
// Every route lives under /<StudentID>/<scenario>
// -----------------------------------------------------------------------------

const router = express.Router({ mergeParams: true });

app.use("/:studentId/:scenario", (req, res, next) => {
  if (!/^\d{8}$/.test(req.params.studentId)) {
    return res.status(404).json({ error: "The path must start with your 8-digit Student ID" });
  }
  req.scenario = scenarios[req.params.scenario];
  if (!req.scenario) return res.status(404).json({ error: "No such scenario" });
  next();
}, router);

// Only the scenario's own collection name is accepted.
router.use("/:collection", (req, res, next) => {
  if (req.params.collection !== req.scenario.name) return next("router");
  next();
});

// Every record.
router.get("/:collection", (req, res) => {
  res.json(req.scenario.items);
});

// Add a record. Every field is required.
let nextId = 100;
router.post("/:collection", (req, res) => {
  const body = req.body ?? {};
  const record = { id: "n" + nextId };
  for (const field of req.scenario.fields) {
    const value = typeof body[field] === "string" ? body[field].trim() : "";
    if (!value) return res.status(400).json({ error: `${field} is required` });
    record[field] = value;
  }
  nextId++;
  req.scenario.items.push(record);
  res.status(201).json(record);
  if (req.scenario === scenarios.orchard) io.emit("harvest:added", record);
});

// Delete one record.
router.delete("/:collection/:id", (req, res) => {
  const index = req.scenario.items.findIndex((i) => i.id === req.params.id);
  if (index === -1) return res.status(404).json({ error: "Nothing with that id" });
  req.scenario.items.splice(index, 1);
  res.sendStatus(204);
  if (req.scenario === scenarios.orchard) io.emit("harvest:removed", { id: req.params.id });
});

app.use((req, res) => res.status(404).json({ error: "No such route" }));

httpServer.listen(PORT, () => {
  console.log(`Week 11 lab API listening on http://localhost:${PORT}`);
  console.log("Use http://localhost:8080/<StudentID>/<scenario> with one of these scenarios:");
  console.log("  " + Object.keys(scenarios).join(", "));
});
