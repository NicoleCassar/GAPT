<?php
$result = fopen("result.txt", 'w');
$arr = array();
$row = 0;
$name = array();
$command = "py SMPAlgtoCSV.py"; //shell command
$esc_command = escapeshellcmd($command);
shell_exec($esc_command); 
$file = fopen('SMPResult.csv', 'r');
while (($line = fgetcsv($file)) !== FALSE) {
  //$line is an array of the csv elements
  $string = implode(", ", $line); //change array to string 
  $str_arr[$row] = explode(",", $string); //change string to array
  $row++;
}
fclose($file);
for($i = 0; $i <= sizeof($str_arr)-1; $i++)
{
	for($j = 0; $j <= sizeof($str_arr)-1; $j++)
	{
		//echo "<br>";
		//print_r($str_arr[$i][$j]);
		if($j == 0){
			$name[$i] = $str_arr[$i][$j];
		}
		elseif($j == 1){
			$tutor[$i] = $str_arr[$i][$j];
		}
		
	}
}

for($z = 0; $z <= sizeof($name)-1; $z++)
{
	$res = $name[$z] . " is assigned to " . $tutor[$z] . "\n";
	fwrite($result,$res);
	// print_r($name[$z]);
	// echo " is assigned to ";
	// print_r($tutor[$z]);
	//echo "<br>";
}
include 'test.html';

//Read CSV file with result from SMP
// For now Just display, later write to DB.
?>

