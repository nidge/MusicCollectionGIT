##################################################################
# Script to extract all songs from the dir specified in the first
# line to 3 csv files.
##################################################################

param ([Parameter(Mandatory=$false,HelpMessage='The directory where the files are held i.e. C:\MyM')] [string]$dir,
       [Parameter(Mandatory=$false,HelpMessage='[Optional] The table to insert the data into')] [string]$table,
       [Parameter(Mandatory=$false,HelpMessage='[Optional] The output (songs) file')] [string]$songsFile,
       [Parameter(Mandatory=$false,HelpMessage='[Optional] The artists file')] [string]$artistFile,
       [Parameter(Mandatory=$false,HelpMessage='[Optional] The albums file')] [string]$albumsFile
       )
      
if(!$dir)
{
    $dir = "c:\MyM"
}

if(!$table)
{
    $table = "MyTable"
}

if(!$songsFile)
{
    $songsFile = "songs.csv"
}

if(!$artistsFile)
{
    $artistsFile = "artists.csv"
}

if(!$albumsFile)
{
    $albumsFile = "albums.csv"
}

cls

if (test-path -path "$dir\$songsFile")
{
    del "$dir\$songsFile"
}

$insertStatement = "Insert into $table Values {"
Write-Output $insertStatement  | Out-File -Append $dir'\'$songsFile

cd $dir
$artists = Get-ChildItem -Path $dir
$count=0

foreach ($artist in $artists)
{
    $albums = Get-ChildItem $artist.Name
    foreach ($album in $albums)
    {
        if ($album.Name -ne $artistsFile -and $album.Name -ne $albumsFile  -and $album.Name -ne $songsFile )
        {
           $tempDir = "$dir" + "\" + $artist.Name + "\" + $album.Name
           $songs = Get-ChildItem -Path $tempDir
           foreach ($song in $songs)
           {
            $count++
            if ($count -gt 995)
            {
                $count=0
                $date = Get-Date
                $songsFile = $songsFile + "_" + $date.Millisecond
            }
            Write-Host "artist = " $artist.Name "album = " $album.Name "song - " $song.Name
            $tempalb = "('1','1', N'" + $artist.Name.Replace("'", "''") + "', N'" + $album.Name.Replace("'", "''") + "', N'" + $song.Name.Replace("'", "''")  + "'),"
            Write-Output $tempalb  | Out-File -Append $dir'\'$songsFile
           }
        }
    }
}

$insertStatement = " } "
Write-Output $insertStatement | Out-File -Append $dir'\'$songsFile