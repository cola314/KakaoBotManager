const mysql = require('mysql');
const sha256 = require('../util/sha256');
const uuid = require('uuid');

const db = mysql.createConnection({
    host: process.env.DB_HOST,
    port: process.env.DB_PORT || 3306,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_DATABASE
});
db.connect();

const query = (sql, args) => {
    return new Promise((resolve, reject) => {
        db.query(sql, args, (err, rows) => {
            if (err) {
                reject(err);
            } else {
                resolve(rows);
            }
        })
    })
}

const initAdmin = () => {
    db.query('INSERT INTO user (id, is_temp_password, password_hash, account_type, guid) VALUES(?, ?, ?, ?, ?)',
        ['admin', true, sha256.ToBase64Hash("1234"), 2, uuid.v4()],
        (err, results, fields) => {
            if (err) {
                console.log('admin already exist');
            } else {
                console.log('init admin success');
            }
        }
    );
};
initAdmin();

module.exports.query = query;