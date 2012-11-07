WP7Klient
=========

Twitter.com baasunktsionaalsust realiseeriv rakendus, mis loodi eesmärgiga õppida tundma 
Windows Phone SDK 7.1(Mango).


Rakenduse käivitamiseks on vajalik
==================================

- Microsot Visual Studio 2010
- Microsoft Windows Phone SDK 7.1
- C# kompilaator

Märkmed realisatsiooni kohta
============================

- Kasutatakse OAuth PIN-koodil baseeruvad identifitseerimist, kusjuures tagastatud PIN
  loetakse sisse ilma kasutaja sekkumiseta.
- Rakenduse käivitamise toimib identifitseerimise ajal teleoni Tagasi nupp veebilehtiseja
  tagasi-nupuna.
- Postituste ja otsingu lehel kuvatavate postitustes (tweetides) leiduvad URLid avatakse
  veebilehitsejas kui vastaval postitusel vajutada.
- Nupud millega alustatakse otsingut või lisatakse postitust muudavad oma värvi andmete
  edastamise ajaks halliks, et anda visuaalset tagasisidet kasutajale.
- Lisateave lehel leiduv nupp "Saada soovitus või ettepanek" kasutab Windows Phone sisse-
  ehitatud e-posti klienti. Emulaatoris antud unktsionaalsust testida ei saa.