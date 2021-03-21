# MeasurementService

Komplex mérés adatgyűjtő rendszer egyedi implementációja.
Felhasznált technológia: .NET Core 3.1
Programozási nyelv:      C#

3 fő részből áll

1. MeasureService (Windows Service, Worker típusú)
    - Socket Client (TCP), hálózaton kapcsolódik a Serverhez
    - Automatikusan csatlakozik a szerverhez és küldi az adatokat
    - Mért adatok: CPU hőmérséklet [°C], Összetett CPU használat [%]  
      (OpenHardwareLibCore vagy System.Managment alapú mérés)
    - Adatok lekérdezése ciklikusan (Periódus idő)
    - Periodus ideje változtatható (app.Config-ba lehet menteni)
    - Naplózott működés(program könyvtárában log.txt-vel Serilog osztállyal)
    <TODO>
      - fogadó oldal nem jól működik
      - az előző miatt a Periódus idő nem változtatható
      - Loggolás nem megfelelő (SeriLog nem működik a paraméterezésnek megfelelően
    </TODO>

2. MeasureServerService (windwos service (még), hiányos a működése)
    - Socket Server a méréseket végző Socket Client-ek csatlakoznak hozzá
    - List-be tárolja a felcsatlakozott Socket Clienteket
    - Naplózás
    - Entity FrameWork használata az adatok mentéséhez
    - Mért értékek tárolása (nem működik megfelelően még)
      (segítség migráláshoz:
        
        - add-migration CreateMeasures  (ha még nincs generálva)
        - update-database –verbose      (adatbázisban elvégzi a műveleteket)
        
        - Remove-Migration              (migráció visszavonása)
    <TODO>
    - Felhasználók tárolása (Név, jelszó)
    - Felhasználók kezelése
    - Kapcsolódás a Frontend rendszerhez
    <TODO>
    
3. Frontend (még nem készült el
 .Net Core 3.1
 Blazor
 Web assembly
