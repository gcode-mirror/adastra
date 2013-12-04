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

	if ($_GET['function'] == "ListExperiments")
	{
		$iduser = $_GET['iduser'];
		// Set a resultset. For testing we are goint to return the full table (3 rows)
		$result = $mysqli->query("select * from experiment where iduser = $iduser order by idexperiment");

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
	else
	if ($_GET['function'] == "ListExperimentsByExperimentIdUserId")
	{
	    $iduser = $_GET['iduser'];
		$idexperiment = $_GET['idexperiment'];
		
		//modified 4/12/2013 not to consider the iduser: iduser = $iduser AND
		$result = $mysqli->query("select * from experiment where idexperiment = $idexperiment order by idexperiment");

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
	}else
	if ($_GET['function'] == "AddExperiment")
	{
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
		$comment = $mysqli->escape_string(urldecode($_GET['comment']));
		$description = $mysqli->escape_string(urldecode($_GET['description']));
		$iduser = $_GET['iduser'];
		$exp_date = $mysqli->escape_string(urldecode($_GET['exp_date']));
		
		$sql = "INSERT INTO experiment (name, comment, description, iduser, exp_date) VALUES ('$name', '$comment','$description', $iduser, STR_TO_DATE('$exp_date','%m/%d/%Y %h:%i:%s'))";
		print ($sql);
		print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}else
	if ($_GET['function'] == "UpdateExperiment")
	{
	    $idexperiment = $_GET['idexperiment'];
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
		$comment = $mysqli->escape_string(urldecode($_GET['comment']));
		$description = $mysqli->escape_string(urldecode($_GET['description']));
		$iduser = $_GET['iduser'];
		$exp_date = $mysqli->escape_string(urldecode($_GET['exp_date']));
		
		print($exp_date);
		
		$sql = "UPDATE experiment set name='$name', comment = '$comment', description = '$description', exp_date = STR_TO_DATE('$exp_date','%m/%d/%Y %h:%i:%s') where idexperiment = $idexperiment";
		print ($sql);
		print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}else
	if ($_GET['function'] == "DeleteExperiment")
	{
	    $idexperiment = $_GET['idexperiment'];
		
		$sql = "delete from experiment where idexperiment = $idexperiment";
		print ($sql);
		print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
?>