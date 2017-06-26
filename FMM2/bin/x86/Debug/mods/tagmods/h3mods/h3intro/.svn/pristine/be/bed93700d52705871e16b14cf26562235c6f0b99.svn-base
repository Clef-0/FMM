color 09
IF EXIST "..\..\..\..\bink_disabled" (
    RMDIR /S /Q "..\..\..\..\bink_disabled"
    IF EXIST "..\..\..\..\bink" (
		erase "..\..\..\..\bink\intro.bik"
		erase "..\..\..\..\bink\loading_loop.bik"
		erase "..\..\..\..\bink\logo_343.bik"
		erase "..\..\..\..\bink\logo_microsoft.bik"
		xcopy "bink" "..\..\..\..\bink"
	) ELSE (
		mkdir "..\..\..\..\bink"
		xcopy "bink" "..\..\..\..\bink"
	)
) ELSE (
    IF EXIST "..\..\..\..\bink" (
		erase "..\..\..\..\bink\intro.bik"
		erase "..\..\..\..\bink\loading_loop.bik"
		erase "..\..\..\..\bink\logo_343.bik"
		erase "..\..\..\..\bink\logo_microsoft.bik"
		xcopy "bink" "..\..\..\..\bink" 
	) ELSE (
		mkdir "..\..\..\..\bink"
		xcopy "bink" "..\..\..\..\bink"
	)
)
echo Halo 3 intro installed!
pause