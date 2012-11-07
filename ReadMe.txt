WP7Klient
=========

Twitter.com baasfunktsionaalsust realiseeriv rakendus, mis loodi eesmärgiga õppida tundma 
Windows Phone SDK 7.1(Mango).


Rakenduse käivitamiseks emulaatoris on vajalik
==============================================

- C# kompilaator
- Microsot Visual Studio 2010
- Microsoft Windows Phone SDK 7.1


Märkmed realisatsiooni kohta
============================

- Kasutatakse OAuth PIN-koodil baseeruvat identifitseerimist, kusjuures tagastatud PIN
  loetakse sisse ilma kasutaja sekkumiseta.
- Kasutaja identifitseerimise ajal toimib telefoni "Tagasi nupp" veebilehtiseja tagasi-
  nupuna.
- Postitustes (tweet'ides) leiduvad URL'id avatakse veebilehtisejas, kui vastavale
  postitusele vajutada.
- Nupud, millega alustatakse otsingut või lisatakse postitus, muudavad andmete edastamise
  ajal halliks. Sellisel viisil antakse kasutajale visuaalset tagasisidet pooleliolevast
  protsessist.
- Lisateabe lehel leiduv nupp "Saada soovitus või ettepanek" kasutab Windows Phone sisse-
  ehitatud e-postiklienti. Emulaatoris kirja saatmise funktsionaalsust testida ei saa.