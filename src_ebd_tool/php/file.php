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

	if ($_GET['function'] == "ListFilesByExperimentSubjectModalityID")
	{
	    $idexperiment = $_GET['idexperiment'];
		$idsubject = $_GET['idsubject'];
		$idmodality = $_GET['idmodality'];
		
		$result = $mysqli->query("select file.idfile,file.filename,file.pathname,file.tags
                            from file,list_file 
                            where file.idfile = list_file.idfile 
                            and idexperiment = $idexperiment
                            and idsubject = $idsubject
                            and idmodality = $idmodality
                            order by list_file.idlist_file");

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
	if ($_GET['function'] == "AddFile")
	{
	    $filename = $mysqli->escape_string(urldecode($_GET['username']));
		$pathname = $mysqli->escape_string(urldecode($_GET['password']));
		
		$sql = "INSERT INTO file (filename, pathname) VALUES ('$filename', '$pathname')";
		
		$result = mysqli_query($mysqli,$sql);
		$row_cnt = $result->num_rows;
		//print($row_cnt);
		
		$idfile = -1;
		$row = $result->fetch_assoc();

	    $idfile = $row["idfile"];

		print(json_encode($idfile));	
	}
?>