import axios from "axios";
import cheerio from "cheerio";
import puppeteer from "puppeteer";
import mongoose from "mongoose";
import { connectionURL } from "../config";
import { torrent } from "../torrent";

mongoose.connect(connectionURL, {
    useUnifiedTopology: true,
    useNewUrlParser: true
});




