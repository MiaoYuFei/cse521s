import mysql from "mysql2"
module.exports = {
    HOST: "localhost",
    USER: "root",
    PASSWORD: "Mypass521",
    DB: "cse521s",
    dialect: "mysql",
    pool: {
      max: 5,
      min: 0,
      acquire: 30000,
      idle: 10000
    }
  };

export default db;