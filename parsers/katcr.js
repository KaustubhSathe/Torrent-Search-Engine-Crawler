const axios = require("axios");
const cheerio = require("cheerio");
const puppeteer = require("puppeteer");
const mongoose = require("mongoose");
const { connectionURL } = require("../config");
const { torrent } = require("../torrent");

mongoose.connect(connectionURL, {
    useUnifiedTopology: true,
    useNewUrlParser: true
});

const baseURL = "https://katcr.to";
const categories = [
    "movies",
    "tv",
    "music",
    "games",
    "apps",
    "anime",
    "other"
];

function getSize(inp){
    if(inp.includes("MB")){
        return parseInt(inp.trim().replace("MB","").trim());
    }

    return parseInt(inp.trim().replace("MB","").trim())*1000;
}

function getDate(inp){
    if(inp.includes("min") || inp.includes("hour")){
        return (new Date()).getTime();
    }
    inp.replace("days","");
    inp.replace("day","");
    return (new Date()).getTime() - parseInt(inp.trim())*24*3600*1000;
}



async function parse(){
    const browser = await puppeteer.launch({
        headless: true,
        args: ["--no-sandbox",'--disable-setuid-sandbox']
    });
    const puppetPage = await browser.newPage();
    for(let cat = 0;cat < categories.length;cat++){
        const category = categories[cat];
        for(let page = 1;page <= 200; page++){
            try{
                const currentPage = `${baseURL}/${category}/${page}`;
                await puppetPage.goto(currentPage);
                const bodyHTML = await puppetPage.evaluate(() => document.body.innerHTML);
                const $ = await cheerio.load(bodyHTML);
                const table = $("#wrapperInner > div.mainpart > table > tbody > tr > td:nth-child(1) > div:nth-child(2) > table > tbody").children().toArray();
                table.forEach((itr,i,arr) => {
                    if(i != 0){
                        const tds = $(itr).children().toArray();
                        let currTorrent = {};
                        currTorrent.Name = $(tds[0]).text().trim().substring(0,$(tds[0]).text().trim().indexOf("Posted by")).trim();
                        currTorrent.Link = baseURL + $($(tds[0]).children().children()[0]).attr("href")?.trim();
                        currTorrent.Size = getSize($(tds[1]).text().trim().trim());
                        currTorrent.UploadDate = getDate($(tds[3]).text().trim().trim());
                        currTorrent._id = currTorrent.Link;
                        currTorrent.Source = "KATCR";
                        currTorrent.Seeders = parseInt($(tds[4]).text().trim());
                        currTorrent.Leechers = parseInt($(tds[5]).text().trim());
                        (new torrent(currTorrent)).save((err)=>{
                            if(err){
                                console.error(err);
                            }else{
                                console.log(`Added ${currTorrent.Name}`);
                            }
                        });   
                        console.log(currTorrent);
                    }
                });
            }catch(err){
                console.error(err);
            }
        }
        
    }
    await browser.close();
}

parse();


