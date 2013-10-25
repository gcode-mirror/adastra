<?php

// Prevent caching.
header('Cache-Control: no-cache, must-revalidate');
header('Expires: Mon, 01 Jan 1996 00:00:00 GMT');

// The JSON standard MIME header.
header('Content-type: application/json');

// This ID parameter is sent by our javascript client.

$jsondata = file_get_contents('php://input');

json_decode($jsondata, true);

// Here's some data that we want to send via JSON.
// We'll include the $id parameter so that we
// can show that it has been passed in correctly.
// You can send whatever data you like.
//$data = array("Hello", $id);

$json = json_decode($jsondata, true);

foreach ($json as $key => $jsons) { // jsons contains one record
     
	 foreach($jsons as $key => $value) 
	 {
         echo $key . ' = ' . $value . ' '; // This will show jsut the value f each key like "var1" will print 9
                       // And then goes print 16,16,8 ...
     }
	 echo "\n\n";
}

?>