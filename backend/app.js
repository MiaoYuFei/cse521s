import express from 'express';
import bodyParser from 'body-parser';

const app = express();
app.use(bodyParser.json());
app.use(express.json());

const db = require("./app/models");
db.sequelize.sync();

app.use(bodyParser.urlencoded({ extended: true }));

//simple route
app.get('/', (req, res) => {
    res.send('Hello World!');
});

app.post('/', (req, res) => {
    const result = {
      title: "POST request with Axios",
      body: "POST request",
      userId: 10,
    };
    res.json(result);
});

//listen for requests
const port = process.env.PORT || 3000;
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});

db.sequelize.sync({ force: true }).then(() => {
  console.log("Drop and re-sync db.");
});