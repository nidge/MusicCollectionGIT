#################################################################
# Script to extract all albums from the dir specified in the first
# line to 3 csv files.
##################################################################

$dir = "H:\My Music"
cls

if (test-path -path "$dir\Album.csv")
{
    del "$dir\Album.csv"
}

cd $dir
$artists = Get-ChildItem -Path $dir

foreach ($artist in $artists)
{
    $albums = Get-ChildItem $artist.Name
    foreach ($album in $albums)
    {
        if ($album.Name -ne 'Artist.csv' -and $album.Name -ne 'Album.csv'  -and $album.Name -ne 'Songs.csv' )
        {
            Write-Host "Album = " $album.Name
            $tempAlbum = "('1', N'" + $artist.Name.Replace("'", "''") + "', N'" + $album.Name.Replace("'", "''") + "',2000,'1','1'), "
            Write-Output $tempAlbum  | Out-File -Append $dir'\Album.csv'
        }
    }  
}