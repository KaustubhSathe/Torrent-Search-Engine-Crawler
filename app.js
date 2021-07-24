var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
import helmet from "helmet";
import mongoose from "mongoose";
import { torrent } from "./torrent";
import { connectionURL } from "./config";
var app = express();


app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(helmet());

mongoose.connect(connectionURL, { useNewUrlParser: true, useUnifiedTopology: true }, (err) => {
  if (err) {
    console.log(err);
  }

  console.log("Connected to mongo database.");
});



// catch 404 and forward to error handler
app.use(function (req, res, next) {
  next(createError(404));
});

// error handler
app.use(function (err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
});



const escapeRegex = (text) => {
  return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
};

app.get("/search", async (req, res) => {
  if (req.query.search) {
    const regex = new RegExp(escapeRegex(req.query.search.toString()), 'gi');
    torrent.find({ "Name": regex }, (err, foundTorrents) => {
      if (err) {
        console.error(err);
      } else {
        res.json(foundTorrents);
      }
    })
  }
});

module.exports = app;
