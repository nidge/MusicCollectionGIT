#################################################################
# Script to extract all artists from the dir specified in the first
# line to 3 csv files.
##################################################################

$dir = "H:\My Music"
cls

if (test-path -path "$dir\Artist.csv")
{
    del "$dir\Artist.csv"
}

cd $dir
$artists = Get-ChildItem -Path $dir

foreach ($artist in $artists)
{
    if ($artist.Name -ne 'Artist.csv' -and $artist.Name -ne 'Album.csv'  -and $artist.Name -ne 'Songs.csv' )
    {
        Write-Host "Artist " = $artist.Name
        $tempArtist = "('" + $artist.Name.Replace("'", "''") + "'), "
        Write-Output $tempArtist | Out-File -Append  $dir'\Artist.csv'
    }
}
