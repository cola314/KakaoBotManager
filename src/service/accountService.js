const db = require('./db');
const sha256 = require('../util/sha256');
const uuid = require('uuid');

const ACCOUNT_TYPE = {
    USER: 1,
    ADMIN: 2
};

const getAccountType = (guid, session) => {
    return new Promise((resolve, reject) => {
        db.query('SELECT account_type, session FROM user WHERE guid = ?', [guid, session])
            .then(rows => {
                if (rows.length > 0) {
                    if (rows[0].session != session || rows[0].session == null) {
                        reject("만료된 세션입니다");
                    } else {
                        resolve(rows[0].account_type);
                    }
                } else {
                    reject("요청을 보낸 계정이 존재하지 않습니다");
                }
            })
    });
}

const login = (id, password) => {
    return new Promise((resolve, reject) => {
        let account;
        let session;

        db.query('SELECT * FROM user WHERE id = ? AND password_hash = ?',
            [id, sha256.ToBase64Hash(password)])
            .then(rows => {
                if (rows.length > 0) {
                    account = rows[0];
                    session = uuid.v4();
                    return db.query('UPDATE user SET session = ? WHERE id = ?', [session, id]);
                } else {
                    throw "로그인에 실패했습니다";
                }
            })
            .then(_ => {
                resolve({
                    success: true,
                    message: "",
                    account: {
                        id: account.id,
                        accountType: account.account_type,
                        isTempPassword: account.is_temp_password,
                        session: session,
                        guid: account.guid
                    }
                });
            })
            .catch(err => {
                resolve({
                    success: false,
                    message: err
                })
            })
    });
};

const logout = (guid, session) => {
    return new Promise((resolve, reject) => {
        db.query(`UPDATE user SET session = NULL WHERE guid = ? AND session = ?`, [guid, session])
            .then(data => {
                resolve({
                    success: true,
                    message: ""
                });
            })
            .catch(err => {
                resolve({
                    success: false,
                    message: err
                });
            });
    });
};

const resetTempPassword = (guid, session, newPassword) => {
    return new Promise((resolve, reject) => {
        db.query('UPDATE user SET password_hash = ?, is_temp_password = FALSE WHERE guid = ? AND session = ? AND is_temp_password = TRUE',
            [sha256.ToBase64Hash(newPassword), guid, session])
            .then(data => {
                resolve({
                    result: true,
                    message: ""
                })
            })
            .catch(err => {
                resolve({
                    result: false,
                    message: err
                })
            });
    });
};

const getAccountList = (sender, session) => {
    return new Promise((resolve, reject) => {
        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.ADMIN) {
                    return db.query('SELECT id, account_type, is_temp_password, guid FROM user');
                } else {
                    throw "권한이 없습니다";
                }
            })
            .then(rows => {
                const accounts = rows.map(x => ({
                    id: x.id,
                    accountType: x.account_type,
                    isTempPassword: x.is_temp_password ? true : false,
                    guid: x.guid
                }));
                resolve({
                    success: true,
                    accountList: accounts
                });
            })
            .catch(err => {
                resolve({
                    success: false,
                    message: err
                });
            })
    });
};

const addAccount = (sender, session, id, tempPassword) => {
    return new Promise((resolve, reject) => {
        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.ADMIN) {
                    return db.query('INSERT INTO user (id, is_temp_password, password_hash, account_type, guid) VALUES(?, ?, ?, ?, ?)',
                        [id, true, sha256.ToBase64Hash(tempPassword), ACCOUNT_TYPE.USER, uuid.v4()]);
                } else {
                    throw "권한이 없습니다";
                }
            })
            .then(_ => {
                resolve({
                    success: true,
                    message: ""
                });
            })
            .catch(err => {
                resolve({
                    success: false,
                    message: err
                });
            })
    });
};

const deleteAccount = (sender, session, guid) => {
    return new Promise((resolve, reject) => {
        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.ADMIN) {
                    return db.query('DELETE FROM user WHERE guid = ?', [guid]);
                } else {
                    throw "권한이 없습니다";
                }
            })
            .then(_ => {
                resolve({
                    success: true,
                    message: ""
                });
            })
            .catch(err => {
                resolve({
                    success: false,
                    message: err
                });
            })
    });
};

const getAccountInfo = (sender, session, guid) => {
    return new Promise((resolve, reject) => {
        let account;

        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.ADMIN || sender == guid) {
                    return db.query('SELECT id, account_type, guid FROM user WHERE guid = ?', [guid]);
                } else {
                    throw "권한이 없습니다";
                }
            })
            .then(rows => {
                if (rows.length > 0) {
                    account = {
                        id: rows[0].id,
                        accountType: rows[0].account_type,
                        guid: rows[0].guid
                    }
                    return db.query('SELECT name, guid, is_use_callback FROM application WHERE owner_guid = ?', [guid]);
                } else {
                    throw "계정이 존재하지 않습니다";
                }
            })
            .then(rows => {
                account.applicationList = rows.map(x => ({
                    name: x.name,
                    guid: x.guid,
                    isUseCallback: x.is_use_callback
                }));
                resolve({
                    success: true,
                    message: "",
                    account: account
                });
            })
            .catch(err => {
                resolve({
                    success: false,
                    message: err
                });
            })

    });
};

const resetPassword = (sender, session, guid, tempPassword) => {
    return new Promise((resolve, reject) => {
        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.ADMIN) {
                    return db.query('UPDATE user SET password_hash = ?, is_temp_password = TRUE WHERE guid = ?', [sha256.ToBase64Hash(tempPassword), guid]);
                } else {
                    throw "권한이 없습니다";
                }
            })
            .then(_ => {
                resolve({
                    success: true,
                    message: ""
                });
            })
            .catch(err => {
                resolve({
                    success: false,
                    message: err
                });
            })
    });
}

module.exports.ACCOUNT_TYPE = ACCOUNT_TYPE;
module.exports.login = login;
module.exports.logout = logout;
module.exports.resetTempPassword = resetTempPassword;
module.exports.getAccountList = getAccountList;
module.exports.addAccount = addAccount;
module.exports.deleteAccount = deleteAccount;
module.exports.getAccountInfo = getAccountInfo;
module.exports.resetPassword = resetPassword;