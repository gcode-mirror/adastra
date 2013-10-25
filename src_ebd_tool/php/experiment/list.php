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

	$iduser = $_GET['iduser'];
	// Set a resultset. For testing we are goint to return the full table (3 rows)
	$result = $mysqli->query("select * from experiment where iduser = $iduser order by idexperiment");

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
?>