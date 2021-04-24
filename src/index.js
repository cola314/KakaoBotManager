const express = require('express');
const app = express();
const dotenv = require('dotenv');

dotenv.config();

app.use(express.json());
app.use((req, res, next) => {
    console.log(req.body);
    next();
});

app.use('/api', require('./router/accountRouter'));
app.use('/api', require('./router/applicationRouter'));
//app.use('/api', require('./router/logRouter'));
app.use('/', express.static(__dirname + '/public'));

app.listen(process.env.PORT || 3000);
console.log(`server listen on ${process.env.PORT || 3000}`);