module.exports = (sequelize, Sequelize) => {
    const Tutorial = sequelize.define("cse521sTag", {
      name: {
        type: Sequelize.STRING
      },
      tag_id: {
        type: Sequelize.STRING
      },
    });
  
    return Tutorial;
  };