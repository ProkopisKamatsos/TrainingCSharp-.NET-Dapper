using EfVsDapper;

var repo = new GameCharacterRepository();
repo.Dapper_Update();
repo.EF_Update();
repo.Dapper_Read();
repo.EF_Read();