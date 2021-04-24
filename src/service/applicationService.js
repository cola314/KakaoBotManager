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

const getApplicationList = (sender, session) => {
    return new Promise((resolve, reject) => {
        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.USER) {
                    return db.query('SELECT * FROM application');
                } else {
                    return db.query('SELECT * FROM application WHERE owner_guid = ?', [sender]);
                }
            })
            .then(rows => {
                const applicationList = rows.map(x => ({
                    name: x.name,
                    guid: x.guid,
                    isUseCallback: x.is_use_callback ? true : false,
                    icon: x.icon
                }));
                resolve({
                    success: true,
                    message: "",
                    applicationList: applicationList
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

const addApplication = (sender, session, name, icon) => {
    return new Promise((resolve, reject) => {
        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.ADMIN) {
                    throw "관리자 계정에 애플리케이션을 추가할 수 없습니다";
                } else {
                    return db.query('INSERT INTO application (name, guid, owner_guid, icon, is_use_callback)',
                        [name, uuid.v4(), sender, icon, false]);
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
            });
    });
};

const getApplication = (sender, session, guid) => {
    return new Promise((resolve, reject) => {
        let application;

        getAccountType(sender, session)
            .then(accountType => {
                if (accountType == ACCOUNT_TYPE.ADMIN) {
                    return db.query('SELECT * FROM application WHERE guid = ?', [guid]);
                } else {
                    return db.query('SELECT * FROM application WHERE guid = ? AND owner_guid = ?', [guid, sender]);
                }
            })
            .then(rows => {
                if (rows.length == 0) {
                    throw "권한이 없거나 존재하지 않는 애플리케이션입니다";
                }
                application = {
                    name: rows[0].name,
                    guid: rows[0].guid,
                    isUseCallback: rows[0].is_use_callback ? true : false,
                    icon: rows[0].icon,
                };
                return db.query('SELECT * FROM callbackoption WHERE application_guid = ?', [application.guid]);
            })
            .then(rows => {
                application.callbackOption = rows.map(x => ({
                    httpMethod: x.http_method,
                    url: x.url
                }));
                resolve({
                    success: true,
                    message: "",
                    application: application
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

const setApplication = (sender, session, guid, name, icon) => {
    return new Promise((resolve, reject) => {
        let isAdmin;

        getAccountType(sender, session)
            .then(accountType => {
                isAdmin = accountType == ACCOUNT_TYPE.ADMIN;
                return db.query('SELECT owner_guid FROM application WHERE guid = ?', [guid]);
            })
            .then(rows => {
                if (rows.length == 0 || rows[0].owner_guid != sender) {
                    throw "권한이 없습니다";
                }
                return db.query('UPDATE application SET name = IF(name IS NOT NULL, ?, name), icon = IF(icon IS NOT NULL, ?, icon) WHERE guid = ?',
                    [name == undefined ? null : name, icon == undefined ? null : icon, guid])
                    .then(_ => {
                        resolve({
                            success: true,
                            message: ""
                        });
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

const deleteApplication = (sender, session, guid) => {
    return new Promise((resolve, reject) => {
        let isAdmin;

        getAccountType(sender, session)
            .then(accountType => {
                isAdmin = accountType == ACCOUNT_TYPE.ADMIN;
                return db.query('SELECT owner_guid FROM application WHERE guid = ?', [guid]);
            })
            .then(rows => {
                if (rows.length == 0 || rows[0].owner_guid != sender) {
                    throw "권한이 없습니다";
                }
                return db.query('DELETE FROM application WHERE guid = ?', [guid])
                    .then(_ => {
                        resolve({
                            success: true,
                            message: ""
                        });
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

const addCallbackOption = (sender, session, guid, callbackOption) => {
    return new Promise((resolve, reject) => {
        let isAdmin;

        getAccountType(sender, session)
            .then(accountType => {
                isAdmin = accountType == ACCOUNT_TYPE.ADMIN;
                return db.query('SELECT owner_guid FROM application WHERE guid = ?', [guid]);
            })
            .then(rows => {
                if (rows.length == 0 || rows[0].owner_guid != sender || !isAdmin) {
                    throw "권한이 없습니다";
                }
                return db.query('INSERT INTO callbackoption (application_guid, http_method, url) VALUES(?, ?, ?)',
                    [uuid.v4(), callbackOption.httpMethod, callbackOption.url])
                    .then(_ => {
                        resolve({
                            success: true,
                            message: ""
                        });
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

const setCallbackOption = (sender, session, guid, preCallbackOption, newCallbackOption) => {
    return new Promise((resolve, reject) => {
        let isAdmin;
        getAccountType(sender, session)
            .then(accountType => {
                isAdmin = accountType == ACCOUNT_TYPE.ADMIN;
                return db.query('SELECT owner_guid FROM application WHERE guid = ?', [guid]);
            })
            .then(rows => {
                if (rows.length == 0 || rows[0].owner_guid != sender || !isAdmin) {
                    throw "권한이 없습니다";
                }
                db.query('UPDATE callbackoption SET http_method = ?, url = ? WHERE http_method = ?, url = ?',
                    [preCallbackOption.httpMethod, preCallbackOption.url, newCallbackOption.httpMethod, newCallbackOption.url])
                    .then(_ => {
                        resolve({
                            success: true,
                            message: ""
                        });
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

const deleteCallbackOption = (sender, session, guid, callbackOption) => {
    return new Promise((resolve, reject) => {
        let isAdmin;

        getAccountType(sender, session)
            .then(accountType => {
                isAdmin = accountType == ACCOUNT_TYPE.ADMIN;
                return db.query('SELECT owner_guid FROM application WHERE guid = ?', [guid]);
            })
            .then(rows => {
                if (rows.length == 0 || onwner_guid != sender || !isAdmin) {
                    throw "권한이 없습니다";
                }
                return db.query('DELETE FROM callbackoption WHERE guid = ? AND http_method = ? AND url = ?',
                    [guid, callbackOption.httpMethod, callbackOption.url])
                    .then(_ => {
                        resolve({
                            success: true,
                            message: ""
                        });
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

module.exports.getApplicationList = getApplicationList;
module.exports.addApplication = addApplication;
module.exports.getApplication = getApplication;
module.exports.setApplication = setApplication;
module.exports.deleteApplication = deleteApplication;
module.exports.addCallbackOption = addCallbackOption;
module.exports.setCallbackOption = setCallbackOption;
module.exports.deleteCallbackOption = deleteCallbackOption;