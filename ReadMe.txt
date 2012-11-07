WP7Klient
=========

Twitter.com baasunktsionaalsust realiseeriv rakendus, mis loodi eesmärgiga õppida tundma 
Windows Phone SDK 7.1(Mango).


Rakenduse käivitamiseks emulaatoris on vajalik
==============================================

- C# kompilaator
- Microsot Visual Studio 2010
- Microsoft Windows Phone SDK 7.1


Märkmed realisatsiooni kohta
============================

- Kasutatakse OAuth PIN-koodil baseeruvad identifitseerimist, kusjuures tagastatud PIN
  loetakse sisse ilma kasutaja sekkumiseta.
- Kasutaja identifitseerimise ajal toimib teleoni Tagasi nupp veebilehtiseja tagasi
  -nupuna.
- Postitustes (tweetides) leiduvad URLid avatakse veebilehtisejas, kui vastaval postitusel
  vajutada.
- Nupud, millega alustatakse otsingut või lisatakse postitus, muudavad andmete edastamise
  ajal halliks. Sellisl viisil antakse kasutajale visuaalset tagasisidet pooleliolevast
  tööprotsessist.
- Lisateave lehel leiduv nupp "Saada soovitus või ettepanek" kasutab Windows Phone sisse-
  ehitatud e-posti klienti. Emulaatoris kirja saatmise funktsionaalsust testida ei saa.