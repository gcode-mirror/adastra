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

		$rows = [];
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
		print(json_encode($rows, JSON_NUMERIC_CHECK));
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

		$rows = [];
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
		print(json_encode($rows, JSON_NUMERIC_CHECK));
	}
	
?>