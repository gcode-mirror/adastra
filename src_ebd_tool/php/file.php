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
	    $filename = urldecode($_GET['filename']);
		$pathname = urldecode($_GET['pathname']);
		
		if ($stmt = $mysqli->prepare("INSERT INTO file (filename, pathname) VALUES (?, ?)")) 
		{
			/* bind parameters for markers */
			$stmt->bind_param("ss", $filename, $pathname);

			/* execute query */
			$stmt->execute();

			/* close statement */
			$stmt->close();
		}

		//$sql = "INSERT INTO file (filename, pathname) VALUES ('$filename', '$pathname')";
		
		//$mysqli->query($sql);
		
	    $idfile = $mysqli->insert_id;
        
		mysqli_close($mysqli);
		
		print(json_encode($idfile));
		//print(urldecode($_GET['pathname']));
        //print($pathname);
	}
	else
	if ($_GET['function'] == "DeleteFilesByFileId")
	{
	    $idfile = $_GET['idfile'];
		
		$sql = "delete from file where idfile = $idfile";
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}
	else
	if ($_GET['function'] == "DeleteFilesByFileIdFromListFile")
	{
	    $idfile = $_GET['idfile'];
		
		$sql = "delete from list_file where idfile = $idfile";
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}
	else
	if ($_GET['function'] == "AssociateFile")
	{
	    $idexperiment = $_GET['idexperiment'];
		$idsubject = $_GET['idsubject'];
		$idmodality = $_GET['idmodality'];
		$idfile = $_GET['idfile'];
		
		$sql = "INSERT INTO list_file (idexperiment, idsubject, idmodality, idfile) VALUES ($idexperiment, $idsubject, $idmodality, $idfile)";
		
		$mysqli->query($sql);

		print(json_encode($idfile));	
	}
?>