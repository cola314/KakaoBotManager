const crypto = require('crypto');

const ToBase64Hash = (text) => {
    return crypto.createHash('sha256')
        .update(text)
        .digest('base64');
}

module.exports.ToBase64Hash = ToBase64Hash;