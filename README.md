BlockBlast – .NET MAUI mängurakenduse aruanne
Projekt: BlockBlast
Autor: Aleksandr Vassiljuk
Tehnoloogiad: C#, .NET MAUI, XAML, SQLite, MVVM
Lähtekood: GitHub – BlockBlast
1. Sissejuhatus
Käesoleva töö eesmärk oli luua interaktiivne mängurakendus .NET MAUI raamistikuga. Arendamisel kasutasin objektorienteeritud programmeerimist (OOP), MVVM-arhitektuurimustrit, andmete salvestamist SQLite andmebaasi, lokaliseerimist ning heliefekte.

Valisin projektiks BlockBlast-nimelise loogikamängu. Mängija paigutab erineva kujuga klotse 8 × 8 mängulauale. Täidetud read ja veerud eemaldatakse ning mängija kogub punkte. Eesmärk on saavutada võimalikult suur punktisumma.

Rakenduses on ka rekordite vaade, seaded, eesti ja inglise keel ning hele ja tume kujundus.
2. Terminite loetelu
- .NET MAUI – Microsofti raamistik platvormiüleste rakenduste loomiseks.
- C# – programmeerimiskeel, milles on kirjutatud mängu loogika.
- OOP – objektorienteeritud programmeerimine, kus kasutatakse klasse ja objekte.
- MVVM (Model–View–ViewModel) – arhitektuurimuster, mis eraldab andmed, kasutajaliidese ja vahepealse loogika.
- XAML – märgistuskeel rakenduse kasutajaliidese kirjeldamiseks.
- SQLite – kohalik andmebaas mängutulemuste salvestamiseks.
- Data Binding – kasutajaliidese sidumine rakenduse andmetega.
- ICommand – liides, mille kaudu saab kasutajaliidese tegevusi ViewModeli käskudega siduda.
- Lokaliseerimine – rakenduse tekstide kohandamine eri keeltele.
- Tap to Place – klotsi valimine ja selle paigutamine mängulauale puudutuse abil.
3. Teoreetiline osa
3.1. MVVM-arhitektuur
Projektis on kasutatud MVVM-põhimõtteid, et mängu andmed ja loogika oleksid kasutajaliidesest eraldatud.
- Model: Block, BlockFactory, GameBoard ja GameRecord kirjeldavad klotse, mängulauda ja tulemusi.
- View: MainPage.xaml, RecordsPage.xaml ja SettingsPage.xaml sisaldavad mängu kasutajaliidest.
- ViewModel: GameViewModel haldab mängu olekut, punktisummat ja mängija tegevusi.
ViewModelis kasutatakse ICommand käske näiteks uue mängu alustamiseks, pausiks, klotsi valimiseks ja paigutamiseks.
3.2. Objektorienteeritud programmeerimine
Mängu loogika on jaotatud klassidesse. Block kirjeldab ühte klotsi, BlockFactory loob klotse ning GameBoard kontrollib nende paigutamist, täidetud ridu ja veerge ning punktide arvestust. Nii on igal klassil kindel ülesanne.
3.3. Andmete salvestamine
Mängutulemuste püsivaks salvestamiseks kasutatakse SQLite andmebaasi. DatabaseService loob vajadusel andmebaasi blockblast.db3 ning sisaldab meetodeid tulemuste salvestamiseks, lugemiseks, parima tulemuse leidmiseks ja kõigi rekordite kustutamiseks.
4. Praktiline osa
4.1. Projekti struktuur
BlockBlast/
├── BlockBlast.slnx
├── BlockBlast/
│   ├── Models/
│   │   ├── Block.cs
│   │   ├── BlockFactory.cs
│   │   ├── GameBoard.cs
│   │   └── GameRecord.cs
│   ├── ViewModels/
│   │   └── GameViewModel.cs
│   ├── Views/
│   │   ├── RecordsPage.xaml
│   │   └── SettingsPage.xaml
│   ├── Services/
│   │   ├── DatabaseService.cs
│   │   ├── LocalizationService.cs
│   │   └── SoundService.cs
│   ├── Localization/
│   │   ├── AppResources.resx
│   │   └── AppResources.en.resx
│   ├── Resources/Raw/
│   │   ├── place.wav
│   │   └── clear.wav
│   ├── MainPage.xaml
│   ├── MainPage.xaml.cs
│   └── MauiProgram.cs
└── README.md
4.2. Mängu toimimine
Mängulaual on 8 × 8 ruutu. Mängija valib ühe pakutavatest klotsidest ning puudutab mängulaual kohta, kuhu soovib selle paigutada. Enne paigutamist kontrollib programm, kas klots mahub lauale ja kas vajalikud ruudud on vabad.

Iga paigutatud ruut annab 10 punkti. Täidetud rea või veeru eemaldamine annab 100 punkti ning lisaks arvestatakse komboboonust. Mängu saab uuesti alustada ja pausile panna. Mäng lõpeb, kui saadaolevaid klotse ei saa enam mängulauale paigutada.
4.3. Rekordid ja SQLite
GameRecord kirjeldab salvestatavat mängutulemust. DatabaseService kasutab SQLiteAsyncConnection ühendust ja võimaldab tulemusi andmebaasi lisada ning rekordite lehel kuvada. Parimat tulemust kasutatakse ka mängu põhivaates.
4.4. Kasutajaliides ja teemad
Kasutajaliides on loodud XAML-i abil. Põhivaates kuvatakse mängulaud, valitavad klotsid, punktid ja mängu olek. Seadete lehel saab muuta mängija nime, helide kasutamist, kujundust ning keelt.

Rakendus toetab Dark ja Light teemat. Kasutaja valikud salvestatakse seadme Preferences salvestusruumi.
4.5. Lokaliseerimine
Rakendus toetab eesti ja inglise keelt. Tõlked asuvad failides AppResources.resx ja AppResources.en.resx. Keelevalikut ja tekstide leidmist haldab LocalizationService.
4.6. Heliefektid ja interaktiivsus
Mängus on kaks helifaili:
- place.wav – klotsi paigutamise heli;
- clear.wav – täidetud rea või veeru eemaldamise heli.
Helide esitamise eest vastutab SoundService. Mängu interaktiivsus põhineb Tap to Place lahendusel: klots valitakse ja paigutatakse mängulauale vajutusega.
4.7. Testimine
Arendamise käigus kontrollisin rakenduse käivitumist, klotside paigutamist, punktide arvestamist ja heliefekte. Samuti proovisin mängu seadeid, keelevahetust ja rekordite vaadet. Tekkinud programmeerimisvead parandasin arendamise käigus.
4.8. Versioonihaldus
Kasutasin Giti ja GitHubi, et salvestada projekti lähtekood. Hoidlas on Visual Studio lahendus, C# klassid, XAML-vaated, tõlkefailid ja heliefektid. Ajutised Visual Studio failid ning bin ja obj kaustad on .gitignore abil välja jäetud.
5. Kokkuvõte
Töö tulemusena valmis BlockBlast mängurakendus, mille arendamisel kasutasin .NET MAUI raamistikku, C# programmeerimiskeelt, MVVM-arhitektuuri ja objektorienteeritud programmeerimist.

Kõige olulisem oli mänguloogika loomine: klotside sobivuse kontrollimine, ridade ja veergude eemaldamine ning punktide arvutamine. Lisaks õppisin paremini kasutama SQLite andmebaasi, .resx lokaliseerimisfaile, rakenduse seadeid ja heliefekte.

Edaspidi võiks mängule lisada uusi mängurežiime, animatsioone ja rohkem erinevaid klotse.
6. Kasutatud kirjandus
1. Microsoft. .NET MAUI documentation.
2. Microsoft. Model-View-ViewModel (MVVM).
3. Microsoft. .NET MAUI localization.
4. Versluis, G. Plugin.Maui.Audio.
5. SQLite. SQLite Documentation.
6. Vassiljuk, A. BlockBlast – lähtekood
