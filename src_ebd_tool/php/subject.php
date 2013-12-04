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

	if ($_GET['function'] == "ListSubjects")
	{
		$iduser = $_GET['iduser'];
		// Set a resultset. For testing we are goint to return the full table (3 rows)
		$result = $mysqli->query("select * from subject where iduser = '$iduser' order by idsubject");

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
	if ($_GET['function'] == "ListSubjectsByExperimentId")
	{
		$iduser = $_GET['iduser'];
		$idexperiment = $_GET['idexperiment'];
		
		//modified 4/12/2013 not to consider iduser: //AND iduser = @iduser
		$result = $mysqli->query("select * from subject where idexperiment = $idexperiment");

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
	if ($_GET['function'] == "AddSubject")
	{
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
		$age = $_GET['age'];
		$sex = $_GET['sex'];
	    $iduser = $_GET['iduser'];
		$idexperiment = $_GET['idexperiment'];
		
		$sql = "INSERT INTO subject (name, age, sex, idexperiment, iduser) VALUES ('$name', $age, $sex, $idexperiment, $iduser)";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	if ($_GET['function'] == "DeleteSubject")
	{
	    $idsubject = $_GET['idsubject'];
		
		$sql = "delete from subject where idsubject = $idsubject";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	if ($_GET['function'] == "UpdateSubject")
	{
	    $idsubject = $_GET['idsubject'];
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
		$age = $_GET['age'];
		$sex = $_GET['sex'];
	    $iduser = $_GET['iduser'];
		$idexperiment = $_GET['idexperiment'];
		
		$sql = "UPDATE subject set name='$name', age=$age, sex=$sex, idexperiment = $idexperiment where idsubject = $idsubject";
		print ($sql);
		print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	
?>