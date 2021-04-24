const express = require('express');
const router = express.Router();
const accountService = require('../service/accountService');

router.post('/login', async (req, res) => {
    res.send(await accountService.login(req.body.id, req.body.password));
});

router.post('/logout', async (req, res) => {
    res.send(await accountService.logout(req.body.sender, req.body.session));
});

router.post('/resetTempPassword', async (req, res) => {
    res.send(await accountService.resetTempPassword(req.body.sender, req.body.session, req.body.newPassword));
});

router.post('/getAccountList', async (req, res) => {
    res.send(await accountService.getAccountList(req.body.sender, req.body.session));
});

router.post('/addAccount', async (req, res) => {
    res.send(await accountService.addAccount(req.body.sender, req.body.session, req.body.id, req.body.tempPassword));
});

router.post('/deleteAccount', async (req, res) => {
    res.send(await accountService.deleteAccount(req.body.sender, req.body.session, req.body.guid));
});

router.post('/getAccountInfo', async (req, res) => {
    res.send(await accountService.getAccountInfo(req.body.sender, req.body.session, req.body.guid));
});

router.post('/resetPassword', async (req, res) => {
    res.send(await accountService.resetPassword(req.body.sender, req.body.session, req.body.guid, req.body.tempPassword));
});

module.exports = router;