using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;


public class DB_Initiation : MonoBehaviour {

    // Use this for initialization
    void Start() {
        string __DBName = "URI=file:PathDB.db";
        IDbConnection _connection = new SqliteConnection(__DBName);
        IDbCommand _command = _connection.CreateCommand();
        string sql;

        _connection.Open();

        /*sql = "CREATE TABLE monsters (id INT PRIMARY KEY, name VARCHAR(20))";
        _command.CommandText = sql;
        _command.ExecuteNonQuery();*/

        _command.Dispose();
        _command = null;

        _connection.Close();
        _connection = null;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
