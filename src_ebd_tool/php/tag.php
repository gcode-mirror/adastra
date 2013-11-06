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

	if ($_GET['function'] == "ListTags")
	{
		$result = $mysqli->query("select * from tag order by idtag");

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
	if ($_GET['function'] == "AddTag")
	{
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
	
		$sql = "INSERT INTO tag (name) VALUES ('$name')";
		
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}else
	if ($_GET['function'] == "DeleteTag")
	{
	    $idtag = $_GET['idtag'];
		
		$sql = "delete from tag where idtag = $idtag";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}else
	if ($_GET['function'] == "UpdateTag")
	{
	    $idtag = $_GET['idtag'];
	    $name = $mysqli->escape_string(urldecode($_GET['name']));
		
		$sql = "UPDATE tag set name='$name' where idtag = $idtag";
		//print ($sql);
		//print("\n");
		$result = mysqli_query($mysqli,$sql);

		mysqli_close($mysqli);
	
	    if ($result)
	      print("true");
		else print("false");
		
	}
?>