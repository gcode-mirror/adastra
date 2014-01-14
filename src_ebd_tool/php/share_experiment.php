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

	if ($_GET['function'] == "ListExperimentsSharedToTheUserByOthers")
	{
		$target_userid = $_GET['target_userid'];
		
		$sql = "select * from experiment where idexperiment in (select idexperiment from shared_experiment_user where target_userid = $target_userid)";
		$result = $mysqli->query($sql);

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
		
		//print ($sql);
		//print("\n");
		
		// Returns the JSON representation of fetched data
		print(json_encode($rows));
	}
	else
	if ($_GET['function'] == "ListTargetUsers")
	{
	    $idexperiment = $_GET['idexperiment'];
		$owner_userid = $_GET['owneruserid'];
	
		//$result = $mysqli->query("SELECT * FROM user WHERE  iduser IN (SELECT target_userid FROM shared_experiment_user WHERE idexperiment=$idexperiment AND owner_userid=$owner_userid)");
		$result = $mysqli->query("SELECT * FROM si_personnel_actuel WHERE  iduser IN (SELECT target_userid FROM shared_experiment_user WHERE idexperiment=$idexperiment AND owner_userid=$owner_userid)");

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
	if ($_GET['function'] == "AddSharedExperiment")
	{
	    $idexperiment = $_GET['idexperiment'];
		$owneruserid = $_GET['owneruserid'];
		$targetuserid = $_GET['targetuserid'];
		
		$sql = "INSERT INTO shared_experiment_user (idexperiment, owner_userid, target_userid) VALUES ($idexperiment, $owneruserid,$targetuserid)";
		print ($sql);
		print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}else
	if ($_GET['function'] == "DeleteSharedExperiment")
	{
	    $idexperiment = $_GET['idexperiment'];
		$owner_userid = $_GET['owner_userid'];
		
		$sql = "delete from shared_experiment_user where idexperiment = $idexperiment and owner_userid = $owner_userid";
		print ($sql);
		print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	else
	if ($_GET['function'] == "ListUserGroups")
	{
		$result = $mysqli->query("SELECT * FROM si_groupe");

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