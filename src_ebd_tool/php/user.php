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
	    $username = mysql_real_escape_string(urldecode($_GET['username']));
		$password = mysql_real_escape_string(urldecode($_GET['password']));
		
		$sql = "SELECT iduser FROM user where username = '$username' AND password = '$password';";
		
		$result = mysqli_query($mysqli,$sql);
		
		$iduser = -1;
		foreach($result as $row) 
		{
			$iduser = $row["iduser"];
		}
		
		// Returns the JSON representation of fetched data
		print(json_encode($iduser, JSON_NUMERIC_CHECK));
	}
?>