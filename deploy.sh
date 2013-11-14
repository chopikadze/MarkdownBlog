cd deploy
rsync -avzR --delete . root@blog.softumus.com:/var/www/george.softumus.com
cd ..

