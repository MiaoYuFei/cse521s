import mysql from 'mysql2/promise';

const dbconn = mysql.createPool({
  host: 'localhost',
  user: 'cse521s',
  password: 'Mypass521',
  database: 'cse521s'
});

export default dbconn;
