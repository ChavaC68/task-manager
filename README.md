# Task Manager

מערכת לניהול משימות הכוללת:
- **צד שרת:** ASP.NET Core 8 עם לוגיקה שכבתית, FluentValidation, נתונים בקובץ JSON.
- **צד לקוח:** Angular 19 עם Bootstrap 5.

---

## הרצת השרת

```bash
cd server/TaskManager.Api
dotnet restore
dotnet run
```

השרת יעלה על **`http://localhost:5000`**.

- **Swagger:** `http://localhost:5000/swagger`
- **API:** `http://localhost:5000/tasks`

---

## הרצת האפליקציה

```bash
cd client
npm install
npm start
```

האפליקציה תיפתח ב-**`http://localhost:4200`**.

> השרת צריך לרוץ **לפני** האפליקציה — אחרת יוצג "אין חיבור לשרת".

---

## הרצה מלאה (שני טרמינלים)

**טרמינל 1:**
```bash
cd server/TaskManager.Api
dotnet run
```

**טרמינל 2:**
```bash
cd client
npm start
```