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

	if ($_GET['function'] == "ListModalities")
	{
		$sql = "select * from modality order by idmodality";
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
		
		// Returns the JSON representation of fetched data
		print(json_encode($rows));
	}
	else
	if ($_GET['function'] == "ListModalitiesByExperimentID")
	{
		$idexperiment = $_GET['idexperiment'];
		
		$sql = "select modality.idmodality,modality.name 
                            from modality,list_modality 
                            where modality.idmodality = list_modality.idmodality 
                            and idexperiment = $idexperiment
                            order by list_modality.idlist_modality";
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
		
		// Returns the JSON representation of fetched data
		print(json_encode($rows));
	}
	else
	if ($_GET['function'] == "UpdateModality")
	{
	    $idmodality = $_GET['idmodality'];
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
		$comment = $mysqli->escape_string(urldecode($_GET['comment']));
		$description = $mysqli->escape_string(urldecode($_GET['description']));
		
		$sql = "UPDATE modality set name='$name', comment = '$comment', description = '$description' where idmodality = $idmodality";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	else
	if ($_GET['function'] == "DeleteModality")
	{
	    $idmodality = $_GET['idmodality'];
		
		$sql = "delete from modality where idmodality = $idmodality";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	else
	if ($_GET['function'] == "AddModality")
	{
	    $idmodality = $_GET['idmodality'];
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
		$comment = $mysqli->escape_string(urldecode($_GET['comment']));
		$description = $mysqli->escape_string(urldecode($_GET['description']));
		
		$sql = "INSERT INTO modality (name, comment, description) VALUES ('$name', '$comment', '$description')";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	else
	if ($_GET['function'] == "AddModalityToExperiment")
	{
	    $idmodality = $_GET['idmodality'];
        $idexperiment = $_GET['idexperiment'];
		
		$sql = "INSERT INTO list_modality (idmodality, idexperiment) VALUES ($idmodality, $idexperiment)";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
	}
	
?>