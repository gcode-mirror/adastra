<?php
	// Set your database configuration here
	$DatabaseServer   = "sql-devel.gipsa-lab.grenoble-inp.fr";
	$DatabaseUser     = "appli_anton";
	$DatabasePassword = "mutra45";
	$DatabaseName     = "bdanton";

	// Set a connection to the database
	$mysqli = new mysqli($DatabaseServer, $DatabaseUser, $DatabasePassword, $DatabaseName);
	
	// Try to connect. Die if error
	if ($mysqli->connect_errno) 
	{
		printf("Connect failed: %s\n", $mysqli->connect_error);
		exit();
	}

	if ($_GET['function'] == "VerifyUserPassword")
	{
	    $username = $mysqli->escape_string(urldecode($_GET['username']));
		$password = $mysqli->escape_string(urldecode($_GET['password']));
		
		//fake password authentication for testing
		$sql = "SELECT iduser FROM si_personnel_actuel where prenom = '$username' AND prenom = '$password'";
		//$sql = "SELECT iduser FROM user where username = '$username' AND password = '$password';";
		
		$result = mysqli_query($mysqli,$sql);
		$row_cnt = $result->num_rows;
		//print($row_cnt);
		
		$iduser = -1;
		$row = $result->fetch_assoc();
		
		//foreach($result as $row) 
		//{
		//    print("yes");
			$iduser = $row["iduser"];
		//}
		
		// Returns the JSON representation of fetched data
		print(json_encode($iduser));
		//print("$username");
		//print("$password");
		//print("$sql");
		//print("$mysqli->connect_errno");
		
	}
	else
	if ($_GET['function'] == "ListUsers")
	{
		$result = $mysqli->query("SELECT * FROM si_personnel_actuel");

		$rows = array();
		// Iterate the resultset to get all data
		while($row = mysqli_fetch_assoc($result)) 
		{
			$rows[] = $row;
		}
		
		// Close the resultset
		$result->close();

		// Close the database connection
		$mysqli->close();
		
		// Returns the JSON representation of fetched data
		print(json_encode($rows));
	}
?>