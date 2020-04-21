SELECT pref.preference_id, pref.student_id, area.title, sup.name, sup.surname
FROM student_preference pref
INNER JOIN supervisor_area area
ON pref.area_id = area.area_id
INNER JOIN supervisor sup
ON area.supervisor_id = sup.supervisor_id
ORDER BY pref.student_id, pref.preference_id;