# Rakenne ja toiminta

## Nauhanlukijat

ESKO lukee ohjelmakoodin reikänauhoilta ja suorittaa sen käsky kerrallaan. Ohjelmakoodia ei ole mahdollista lukea keskusmuistiin ja suorittaa sieltä. Nauhanlukijoita on kaikkiaan 10 kpl ja käskyllä a n voidaan siirtyä nauhalta toiselle ja käskyllä a * palata edelliselle nauhalle. Täten eri nauhoille voidaan sijoittaa esim. aliohjelmia. Mikäli ohjelman halutaan muodostavan silmukkarakenteen, voidaan nauha liimata renkaaksi.

Nauhanlukijoita käytetään myös laskettavien lukujen sisäänvientiin. Tämä on välttämätöntä, sillä laskettavien lukujen on sijaittava keskusmuistissa. Tällöin sisäänvietävät luvut voidaan kirjoittaa nauhalle ESKOon kuuluvalla kirjoituskoneella ja ajaa toiselta nauhalta ohjelmaa, joka lukee luvut toiselta nauhalta ja sijoittaa ne muistiin haluttuun paikkaan.

## Muisti

ESKOn muisti on pyörivä magneettirumpu, jonka pinnalle luvut varastoidaan. Muistirumpu jakautuu 30 uraan ja jokainen ura taas 60 sektoriin. Urat on numeroitu 00 .. 29 ja sektorit 00 .. 59. Muistin laajuus on siis 30 x 60 = 1 800 muistilokeroa. Tämän päämuistin lisäksi on ESKOlla ns. työmuisti, joka käsittää 40 muistilokeroa. Täten kaikkiaan voidaan 1840 lukua viedä muistiin.

Muistiosoitteet ovat 2- tai 3-numeroisia. 2-numeroiset osoitteet ovat muotoa 60 .. 99 ja ne osoittavat em. työmuistiin.

3-numeroisissa osoitteissa kaksi ensimmäistä numeroa kertovat osoitettavan sektorin numeron ja kolmas numero käytettävän uranvalitsimen. Uranvalitsimia on 10 kpl (0..9). Osoite 123 merkitsee siis sektorissa 12 olevaa muistilokeroa sillä uralla, johon uranvalitsija 3 on asetettu.

Käskyllä v asetetaan haluttu uranvalitsin halutulle uralle. Tällä tavalla ESKO:n ohjelmoinnissa voidaan osoittaa kaikki 1840 muistipaikkaa käyttämällä vain kahta tai kolmea numeroa.

## Laskenta

ESKO laskee sisäisesti binäärijärjestelmässä, mutta laskun alkaessa lukuja koneeseen vietäessä ei niitä tarvitse erikseen muuttaa desimaalijärjestelmästä mainittuun järjestelmään. Luvut annetaan desimaalijärjestelmässä ja kone itse muuttaa ne binäärijärjestelmään. Samoin lopputulokset saadaan muutettuina desimaalijärjestelmään. Luvut viedään koneeseen ns. puolilogaritmisina, eli muodossa a * 10<sup>b</sup>, jossa mantissa a on välillä -1 .. +1 sekä voi normaalitapauksessa käsittää korkeintaan 13 desimaalia. Eksponentti b voi vaihdella välillä ±38. ESKO pystyy laskemaan sekä liikkuvalla pilkulla, jolloin eksponentti b voi vaihdella, että kiinteällä pilkulla, jolloin b on nolla.

## Tulostus

Eri kirjoituskäskyjä käyttäen saadaan tulokset halutussa muodossa, painettuina tai rei'itettyinä. Samoin saadaan
tulokset halutulla desimaalimäärällä. Käsky k n8 on tärkeä, sillä tällä käskyllä tulokset saadaan valmiiksi
nauhalle rei'itettynä ja siinä muodossa kuin lukujen tulee olla muistiin vietäessä. 

Kirjoituskäskyt ovat melko hitaita (13-14 merkkiä sekunnissa) verrattuna käskyjen suoritusnopeuteen (n. 100 käskyä sekunnissa). ESKO kykenee
kirjoituksen aikana suorittamaan jo seuraavaa käskyä. Täten kirjoituskäskyjä ei kannata sijoittaa ohjelmakoodiin monta peräkkäin.

## Substituutio

Erityisellä tulostuskäskyllä (k n9) voidaan korvata (substituoida) ohjelmakoodissa olevia osoitteita laskennan perusteella. Käsky siirtää tulostettavan luvun desimaalimuodossa ns. tulosrekisteriin varsinaisesti tulostamatta sitä ja ESKO jatkaa käskyjen suorittamista normaalisti kunnes löytää käskyn, jossa osoitteen numeroita on korvattu *-merkillä. Tällöin nämä osoitteen *-merkit korvataan em. rekisterissä olevan luvun desimaaleilla. Käskyn k n9 ei tarvitse olla välittömästi sen käskyn edellä, johon sijoitus tapahtuu, vaan välillä saa olla miten monta käskyä tahansa. Sijoitus suoritetaan heti ensimmäiseen käskyyn, jossa *-merkki on, eikä enää muihin käskyihin.

Substituutiota voi käyttää myös lukijalaitteiden valintaan. Jos käskyn k n9 jälkeen tulee käsky a *, määrittää tulosrekisterissä sillä hekellä olevan luvun ensimmäinen numero käynnistyvän lukijalaitteen. Näin laskutoimituksen perusteella on mahdollista vaihtaa mihin aliohjelmaan siirrytään.

## Syklinen permutaatio

Käskyllä s nN saadaan aikaan ns. syklinen permutaatio. Urassa, joka on kytketty valitsijaan N vaihdetaan sektorien 1 .. n sisältö siten, että sektorin 2 sisältö kirjoitetaan 1:een, sektorin 3 sisältö 2:een jne. Vaikka ESKO:sta puuttuu varsinainen osoitinaritmetiikka, tällä käskyllä voidaan tätä puutetta hieman kiertää. Sen sijaan, että osoitetta inkrementoitaisiin, muutetaankin itse muistin sisältöä.

# Käskykanta

## Aritmeettiset operaatiot

{| {{prettytable}}
! Käsky
! Kuvaus
|-
| + n
| Laske luvut yhteen akusta ja muistiosoitteessa n. Tulos tallennetaan akkuun
|-
| - n 
| Vähennä akusta luku osoitteesta n ja tallenna tulos akkuun.
|-
| m nm
| (akk) + (n) * (m) → akk
|-
| <u>m</u> nm
| (akk) - (n) * (m) → akk
|-
| x n	
| (akk) * (n) → akk
|-
| <u>x</u> n
| -(akk) * (n) → akk
|-
| : n
| (akk) : (n) → akk
|-
| j
| neliöjuuri(akk) -> akk
|-
| r
| Ota itseisarvo akusta ja tallenna akkuun.
|}

## Talletusoperaatiot

{| {{prettytable}}
! Käsky
! Kuvaus
|-
| °→ n
| Siirrä luku akusta muistiosoitteeseen n. Nollaa akku.
|-
| → n
| Siirrä luku akusta muistiosoitteeseen n.
|-
| s nN
| Syklinen permutaatio
|-
| v xn
| Kytke uranvalitsija x uraan n. (x: 0-9, n: 0-29)
|-
| *
| Varusta seuraavan °→n tai →n käskyn luku tunnusmerkillä
|}

## Kirjoitusoperaatiot

{| {{prettytable}}
! Käsky
! Kuvaus
|-
| k nw
| Kirjoita (akk) esille (1/2)n desimaalin tarkkuudella sellaisesa muodossa kuin w osoittaa; akk nollataan.
|-
| k n0-3
| Kirjoitetaan luku kirjoittimella.
|-
| k n4-8
| Rei'itetään luku reikänauhanlävistimellä.
|-
| k n9
| Substituutiota edeltävä käsky.
|-
|t
| Tabulointi
|-
| at
| Kirjoituskoneen telavaunun palautus
|}

## Nauhanlukijoiden ohjaus

{| {{prettytable}}
! Käsky
! Kuvaus
|-
| a n
| Käynnistä lukijalaite n (n = 0...9)
|-
| a *
| Käynnistä äsken käynnissä ollut (edellinen) lukijalaite (Huom: * tai e luvun lopussa sisään syöttäessä saa aikaan käskyn a *)
|-
| aa
| Pysähdy
|-
| c
| Pysähdy, jos katkaisija "c" on valvontapöydässä "on" asennossa.
|}

## Hyppykäskyt

{| {{prettytable}}
! Käsky
! Kuvaus
|-
| ee
| Ehdollisen hypyn loppu
|-
| e 0
| Jos mant. etumerkki on akussa negatiivinen, hyppää seuraavaan ee:hen.
|-
| e 1
| Jos mant. etumerkki on akussa positiivinen, hyppää seuraavaan ee:hen.
|-
| e 2
| Jos eksponentin etumerkki on akussa negatiivinen: hyppää seuraavaan ee:hen.
|-
| e 3
| Jos eksponentin etumerkki on akussa positiivinen: hyppää seuraavaan ee:hen.
|-
| e 4
| Jos viimeksi muistista luettu luku ei ollut varustettu tunnusmerkillä tai käsky * ei ollut annettu juuri edellä: hyppää seuraavaan ee:hen.
|-
| e 5
| Jos viimeksi muistista luettu luku oli varustettu tunnusmerkillä tai käsky * ei ollut annettu juuri edellä: hyppää seuraavaan ee:hen.
|-
| e 6
| Jos suuntaaja S 1 on valvontapöydässä "on"-asennossa: hyppää seuraavaan ee:hen.
|-
|e 7
| Jos suuntaaja S 2 on valvontapöydässä "on"-asennossa: hyppää seuraavaan ee:hen.
|}

## Laskutapaa koskevat käskyt

{| {{prettytable}}
! Käsky
! Kuvaus
|-
| av0
| Laske käyttäen yksinkertaista mantissan pituutta ja liikkuvaa pilkkua.
|-
| av1
| Laske käyttäen kaksinkertaista mantissan pituutta ja liikkuvaa pilkkua.
|-
| av2
| Laske käyttäen yksinkertaista mantissan pituutta ja kiinteää pilkkua.
|-
| av3
| Laske käyttäen kaksinkertaista mantissan pituutta ja kiinteää pilkkua.
|-
| av4
| Rei'itä kaikki seuraavat käskyt e:hen asti, vieläpä e, suorittamatta niitä.
|}

## Korjaus

{| {{prettytable}}
! Käsky
! Kuvaus
|-
| f
| Virheellisen lävistyksen poisto. Tämä merkki ei vaikuta koneeseen lainkaan.
|}

# Lähteet

* Carlsson, Tage: Matematiikkakomitean ESKO. Teknillinen aikakauslehti 2/1959.
* Carlsson, Tage: On matematikmaskinerna G1a och ESKO. Arkhimedes 1957.
* Ahokas, Osmo: Normaaliyhtälöiden ratkaisun ohjelmointi matematiikkakone ESKOlle. Teknillisen korkeakoulun lopputyö, Helsinki 1959.
