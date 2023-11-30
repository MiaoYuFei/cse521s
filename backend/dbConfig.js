import mysql from 'mysql2/promise';

let production = false;

if (process.env.ENV) {
  if (process.env.ENV === "production") {
    production = true;
  }
}

const dbconn =
production ?
mysql.createPool({
  host: 'cse521s.clkofmhgqjwu.us-east-2.rds.amazonaws.com',
  user: 'cse521s',
  password: 'Mypass521',
  database: 'cse521s'
})
:
mysql.createPool({
  host: 'localhost',
  user: 'cse521s',
  password: 'Mypass521',
  database: 'cse521s'
});

export default dbconn;
