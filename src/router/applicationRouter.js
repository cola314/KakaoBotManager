const express = require('express');
const router = express.Router();
const applicationService = require('../service/applicationService');

router.post('/getApplicationList', async (req, res) => {
    res.send(await applicationService.getApplicationList(req.body.sender, req.body.session));
});

router.post('/addApplication', async (req, res) => {
    res.send(await applicationService.addApplication(req.body.sender, req.body.session, req.body.name, req.body.icon));
});

router.post('/getApplication', async (req, res) => {
    res.send(await applicationService.getApplication(req.body.sender, req.body.session, req.body.guid));
});

router.post('/setApplication', async (req, res) => {
    res.send(await applicationService.setApplication(req.body.sender, req.body.session, req.body.guid, req.body.name, req.body.icon));
});

router.post('/deleteApplication', async (req, res) => {
    res.send(await applicationService.deleteApplication(req.body.sender, req.body.session, req.body.guid));
});

router.post('/addCallbackOption', async (req, res) => {
    res.send(await applicationService.addCallbackOption(req.body.sender, req.body.session, req.body.guid, req.body.callbackOption));
});

router.post('/setCallbackOption', async (req, res) => {
    res.send(await applicationService.setCallbackOption(req.body.sender, req.body.session, req.body.guid, req.body.preCallbackOption, req.body.newCallbackOption));
});

router.post('/deleteCallbackOption', async (req, res) => {
    res.send(await applicationService.deleteCallbackOption(req.body.sender, req.body.session, req.body.guid, req.body.callbackOption));
});

module.exports = router;