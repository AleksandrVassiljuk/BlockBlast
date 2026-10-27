# BlockBlast – .NET MAUI mängurakenduse aruanne

**Projekt:** BlockBlast  
**Autor:** Aleksandr Vassiljuk  
**Tehnoloogiad:** C#, .NET MAUI, XAML, SQLite, MVVM  
---

## 1. Sissejuhatus

Käesoleva töö eesmärk oli luua interaktiivne mängurakendus, kasutades .NET MAUI raamistikku. Arendamise käigus kasutasin objektorienteeritud programmeerimist (OOP), MVVM-arhitektuurimustrit, SQLite andmebaasi, lokaliseerimist ning heliefekte.

Valisin projektiks **BlockBlast**-nimelise loogikamängu. Mängija ülesanne on paigutada erineva kujuga klotse 8 × 8 mängulauale. Kui mängija täidab terve rea või veeru, eemaldatakse see mängulaualt ning mängija teenib punkte. Mängu eesmärk on koguda võimalikult suur punktisumma ja saavutada uus rekord.

Rakenduses on olemas rekordite vaade, seadete leht, eesti ja inglise keele valik ning hele ja tume kujundus.

## 2. Terminite loetelu

- **.NET MAUI** – Microsofti raamistik platvormiüleste mobiili- ja töölauarakenduste loomiseks.
- **C#** – programmeerimiskeel, mida kasutasin mängu loogika kirjutamiseks.
- **OOP** – objektorienteeritud programmeerimine, mis põhineb klassidel ja objektidel.
- **MVVM (Model-View-ViewModel)** – arhitektuurimuster, mis eraldab andmed, kasutajaliidese ja rakenduse loogika.
- **XAML** – märgistuskeel rakenduse kasutajaliidese loomiseks.
- **SQLite** – kohalik andmebaas mängutulemuste salvestamiseks.
- **Data Binding** – kasutajaliidese sidumine rakenduse andmetega.
- **ICommand** – liides, mille abil saab kasutajaliidese tegevusi siduda ViewModeli käskudega.
- **Lokaliseerimine** – rakenduse tekstide kohandamine erinevatele keeltele.
- **Tap to Place** – funktsioon, mis võimaldab klotsi valida ja puudutusega mängulauale paigutada.

## 3. Teoreetiline osa

### 3.1. MVVM-arhitektuur

BlockBlast projekti arendamisel kasutasin MVVM-arhitektuurimustrit. Selle eesmärk on hoida mängu andmed, loogika ja kasutajaliides eraldi, et kood oleks paremini organiseeritud ja lihtsamini hallatav.

MVVM koosneb kolmest osast:

- **Model** – sisaldab mängu andmeid ja loogikat. Minu projektis on selleks Block, BlockFactory, GameBoard ja GameRecord.
- **View** – kasutajaliides, mille kaudu mängija rakendust kasutab. Selleks on MainPage, RecordsPage ja SettingsPage.
- **ViewModel** – ühendab kasutajaliidese mängu loogikaga. GameViewModel haldab mängu olekut, punktisummat ja mängija tegevusi.

ViewModelis kasutatakse ICommand käske näiteks uue mängu alustamiseks, pausile panemiseks ja klotside paigutamiseks.

### 3.2. Objektorienteeritud programmeerimine

Mängu loogika on jagatud erinevateks klassideks, millel on kindlad ülesanded.

**Block** kirjeldab mängus kasutatavat klotsi. **BlockFactory** loob erineva kujuga klotse ning **GameBoard** kontrollib klotside paigutamist, täidetud ridade ja veergude eemaldamist ning punktide arvutamist.

Selline lahendus muudab koodi loetavamaks ja võimaldab mängu funktsioone lihtsamini täiendada.

### 3.3. Andmete salvestamine

Mängutulemuste salvestamiseks kasutasin SQLite andmebaasi.

Projektis on loodud DatabaseService klass, mis kasutab SQLiteAsyncConnection ühendust. Andmebaasi fail on **blockblast.db3**.

DatabaseService võimaldab mängutulemusi salvestada, lugeda, leida parima tulemuse ning vajadusel rekordid kustutada.

SQLite kasutamine võimaldab säilitada mängutulemusi ka pärast rakenduse sulgemist.

## 4. Praktiline osa

### 4.1. Projekti struktuur

BlockBlast projekt on loodud Visual Studios ning jagatud erinevateks kaustadeks.

**Models/** sisaldab mängu andmemudeleid ja loogikat:

- Block.cs
- BlockFactory.cs
- GameBoard.cs
- GameRecord.cs

**ViewModels/** sisaldab GameViewModel.cs faili, mis haldab mängu olekut ja kasutaja tegevusi.

**Views/** sisaldab kasutajaliidese lehti:

- RecordsPage.xaml
- RecordsPage.xaml.cs
- SettingsPage.xaml
- SettingsPage.xaml.cs

**Services/** sisaldab rakenduse teenuseid:

- DatabaseService.cs – andmebaasi haldamine.
- LocalizationService.cs – keelte haldamine.
- SoundService.cs – heliefektide esitamine.

**Localization/** sisaldab eesti ja inglise keele tõlkefaile.

**Resources/Raw/** sisaldab mängu helifaile place.wav ja clear.wav.

**MainPage.xaml** ja **MainPage.xaml.cs** sisaldavad mängu peamist kasutajaliidest.

### 4.2. Mängu toimimine

BlockBlast mängulaual on 8 × 8 ruutu. Mängijale kuvatakse erineva kujuga klotse, mida saab mängulauale paigutada.

Mängija valib ühe klotsi ja seejärel vajutab mängulaual kohta, kuhu soovib selle paigutada.

Enne paigutamist kontrollib programm, kas klots mahub valitud kohta ning kas vajalikud ruudud on vabad.

Iga paigutatud ruut annab **10 punkti**. Täidetud rea või veeru eemaldamine annab **100 punkti** ning lisaks on mängus komboboonus.

Mängija saab alustada uut mängu, panna mängu pausile ja jätkata mängimist.

Mäng lõpeb siis, kui saadaolevaid klotse ei ole enam võimalik mängulauale paigutada.

### 4.3. Rekordid ja SQLite andmebaas

Mängutulemuste salvestamiseks kasutasin GameRecord andmemudelit ja DatabaseService teenust.

Andmebaasi salvestatakse mängija tulemused, mida saab hiljem rekordite lehel vaadata.

Rekordite kuvamiseks on loodud RecordsPage. Parimat punktisummat kuvatakse ka mängu põhivaates.

Andmebaasi kasutamine võimaldab mängijal jälgida oma varasemaid tulemusi ja proovida saavutada paremat rekordit.

### 4.4. Kasutajaliides ja teemad

Mängu kasutajaliides on loodud XAML-i abil.

Põhivaates kuvatakse mängulaud, valitavad klotsid, punktisumma, rekord ning mängu olek.

Seadete lehel saab kasutaja muuta mängija nime, helide kasutamist, keelt ja kujundust.

Rakendus toetab kahte kujundust:

- **Light Mode** – hele kujundus.
- **Dark Mode** – tume kujundus.

Kasutaja valikud salvestatakse seadme Preferences salvestusruumi, et need säiliksid ka rakenduse järgmisel käivitamisel.

### 4.5. Lokaliseerimine

BlockBlast mäng toetab kahte keelt: eesti ja inglise keelt.

Lokaliseerimiseks kasutasin .resx ressursifaile:

- **AppResources.resx** – eestikeelsed tekstid.
- **AppResources.en.resx** – ingliskeelsed tekstid.

Keelevaliku ja tõlgete kasutamise eest vastutab LocalizationService klass.

Kasutaja saab keelt muuta seadete lehel.

### 4.6. Heliefektid ja interaktiivsus

Mängu huvitavamaks muutmiseks lisasin kaks heliefekti.

- **place.wav** – heli, mis mängib klotsi paigutamisel.
- **clear.wav** – heli, mis mängib täidetud rea või veeru eemaldamisel.

Helide esitamiseks kasutasin Plugin.Maui.Audio paketti ja SoundService klassi.

Mängus on kasutusel Tap to Place funktsioon. Mängija valib esmalt klotsi ning seejärel vajutab mängulaual kohta, kuhu soovib selle paigutada.

See võimaldab mängu juhtida lihtsalt ja mugavalt.

### 4.7. Testimine

Arendamise käigus kontrollisin mängu erinevaid funktsioone.

Testisin rakenduse käivitumist, klotside valimist ja paigutamist, punktide arvutamist ning täidetud ridade ja veergude eemaldamist.

Samuti kontrollisin heliefektide töötamist, mängu pausile panemist, keele vahetamist, kujunduse muutmist ning rekordite vaadet.

Arendamise ajal tekkis erinevaid programmeerimisvigu, mille parandamise käigus õppisin paremini tundma .NET MAUI rakenduse ülesehitust ja C# programmeerimist.

### 4.8. Versioonihaldus ja GitHub

Projekti versioonihalduseks kasutasin Giti ja GitHubi.

GitHubi laadisin Visual Studio lahenduse, C# klassid, XAML-failid, lokaliseerimisfailid ja heliefektid.

Kasutasin ka .gitignore faili, et jätta versioonihaldusest välja ajutised Visual Studio failid ning bin ja obj kaustad.

**Projekti link:** https://github.com/AleksandrVassiljuk/BlockBlast

## 5. Kokkuvõte

Töö käigus valmis BlockBlast mängurakendus, mille arendamisel kasutasin .NET MAUI raamistikku ja C# programmeerimiskeelt.

Õppisin paremini kasutama MVVM-arhitektuurimustrit, objektorienteeritud programmeerimist, SQLite andmebaasi, lokaliseerimist ja heliefekte.

Kõige suuremaks väljakutseks oli mänguloogika loomine. Oluline oli tagada, et klotsid paigutuksid õigesti, täidetud read ja veerud eemaldataks ning punktid arvutataks korrektselt.

Lisaks sain kogemuse kasutajaliidese loomisel XAML-i abil ning rakenduse seadete salvestamisel.

Töö tulemusena valmis interaktiivne mäng, milles saab koguda punkte, vaadata rekordeid, muuta keelt ja kujundust ning kasutada heliefekte.

Edaspidi võiks mängule lisada uusi mängurežiime, animatsioone ja rohkem erinevaid klotsikujundeid.

## 6. Kasutatud kirjandus

1. Microsoft. *.NET MAUI documentation.*  
   https://learn.microsoft.com/en-us/dotnet/maui/

2. Microsoft. *Model-View-ViewModel (MVVM).*  
   https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm

3. Microsoft. *.NET MAUI localization.*  
   https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/localization

4. Versluis, G. *Plugin.Maui.Audio.*  
   https://github.com/jfversluis/Plugin.Maui.Audio

5. SQLite. *SQLite Documentation.*  
   https://www.sqlite.org/docs.html

6. Vassiljuk, A. (2026). *BlockBlast – projekti lähtekood.*  
   https://github.com/AleksandrVassiljuk/BlockBlast
