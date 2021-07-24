import mongoose from "mongoose";

const schema = new mongoose.Schema({
    Seeders: Number,
    Leechers: Number,
    Source: {
        type: String,
        trim: true
    },
    Name: {
        type: String,
        trim: true
    },
    Link: {
        type: String,
        trim: true
    },
    _id: {
        type: String,
        trim: true
    },
    Size: Number,
    UploadDate: Number
}, { versionKey: false });

export const torrent = mongoose.model('torrents', schema);