const mongoose = require("mongoose");
const puppeteer = require("puppeteer");
const cheerio = require("cheerio");
const { connectionURL } = require("../config");
const { torrent } = require("../torrent");

mongoose.connect(connectionURL, {
    useUnifiedTopology: true,
    useNewUrlParser: true
});

const baseURL = "https://www1.thepiratebay3.to";

function getSize(size) {
    if (size == null) return null;
    let type = size.substring(size.length - 3);
    size = size.substring(0, size.length - 4);
    if (type == "GiB") {
        return parseInt(Math.ceil(parseFloat(size) * 1000));
    }
    return parseInt(Math.ceil(parseFloat(size)));
}

function getLink(url) {
    return baseURL + url;
}

function getDate(date) {
    if (date == null) return null;
    date = date.split(" ")[1];
    if (date.includes("Today") || date.includes("ago")) {
        return (new Date()).getTime();
    } else if (date.includes("Y-day")) {
        return (new Date()).getTime() - 86400000;
    } else {
        if (date.length == 5) {
            const thisYear = new Date().getFullYear();
            return Date.parse(`${thisYear}-${date}`);
        } else {
            return Date.parse(date);
        }
    }
}


async function tpbParser() {
    const browser = await puppeteer.launch({
        headless: true,
        args: ["--no-sandbox",'--disable-setuid-sandbox']
    });
    const page = await browser.newPage();
    for (let category = 100; category <= 600; category += 100) {
        for (let pageNumber = 1; pageNumber <= 200; pageNumber++) {
            await page.goto(`${baseURL}/browse/${category}/page/${pageNumber}/`);
            let bodyHTML = await page.evaluate(() => document.body.innerHTML);
            const $ = await cheerio.load(bodyHTML);
            const rows = $("#searchResult > tbody").children();
            for (let i = 0; i < rows.length - 1; i++) {
                const currentRow = $(rows[i]);
                let currTorrent = {};
                currTorrent.Seeders = parseInt($(currentRow.children()[2]).text());
                currTorrent.Leechers = parseInt($(currentRow.children()[3]).text());
                currTorrent.Source = "TPB";
                currTorrent.Name = $(currentRow.children()[1]).find(".detName").text();
                currTorrent.Link = getLink($(currentRow.children()[1]).find(".detLink").attr("href"));
                currTorrent._id = currTorrent.Link;
                currTorrent.Size = getSize($(currentRow.children()[1]).find(".detDesc").text().split(",")[1].trim());
                currTorrent.UploadDate = getDate($(currentRow.children()[1]).find(".detDesc").text().split(",")[0].trim());
                (new torrent(currTorrent)).save((err)=>{
                    if(err){
                        console.error(err);
                    }else{
                        console.log(`Added ${currTorrent.Name}`);
                    }
                });   
                console.log(currTorrent);
            }
        }
    }
    await browser.close();
}


tpbParser();
