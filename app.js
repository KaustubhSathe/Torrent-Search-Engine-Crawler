const express = require('express');
require('dotenv').config()
const cookieParser = require('cookie-parser');
const helmet = require("helmet");
const mongoose = require("mongoose");
const {torrent} = require("./torrent");
const cors = require("cors");
const { connectionURL } = require("./config");
const app = express();
const port = process.env.PORT || 5000;

app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(helmet());

// console.log(connectionURL)

mongoose.connect(connectionURL, { useNewUrlParser: true, useUnifiedTopology: true }, (err) => {
  if (err) {
    console.log(err);
  }else{
    console.log("Connected to mongo database.");
  }
});


const escapeRegex = (text) => {
  return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
};

app.get("/search", (req, res) => {
  if (req.query.search) {
    const regex = new RegExp(escapeRegex(req.query.search.toString()), 'gi');
    // console.log(req.query.search);
    torrent.find({ "Name": regex }, (err, foundTorrents) => {
      if (err) {
        console.error(err);
      } else {
        res.json(foundTorrents);
      }
    })
  }
});

app.get("/", (req, res) => {
  res.send("Service working");
});

app.listen(port,() => console.log(`listening on port ${port}`));
