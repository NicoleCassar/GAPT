import csv #for reading a csv file

i = 0 #to iterate a 2D array for row in reader
k = 0
y = 1
pref = []
pref_supervisor = []
student = []
audit = open("Allocation_Log.txt", 'w')
#pathfile = open("path.txt", 'r')
#path = pathfile.read()
csv_file = open("students.csv", 'r') #CSV file being read from
csv_test = open("SMPResult.csv", 'w') #CSV file to write the results to.
writer = csv.writer(csv_test, delimiter=",")
reader = csv.reader(csv_file)
for row in reader: #Each element with the csv file is read and saved below:
    student.append([]) # Adding a new 2D element with each iteration.
    name = row[1]
    ID = row[3]
    pref = row[5]
    for st in student: 
        if(any(ID in st for st in student)):
            student[i-y].append(pref)
            y+=1
            break;
        if(any(ID not in st for st in student)):
            y=1
            student[i].append(ID)
            student[i].append(name)
            student[i].append(pref)
            break;
    i += 1

student = [x for x in student if x!= []]

for i in range(0, len(student)):
    student[i].append([])

supervisor = []
area = []
supervisors_file = open("supervisors.csv", 'r') #CSV file being read from
tread = csv.reader(supervisors_file)
for row in tread: #Reading all rows from supervisor csv
    supervisor.append([])
    area.append([])
    name = row[0] #supervisor name
    supervisor_id = row[1] #supervisor id
    area_of_supervisor = row[2] #area of research
    quota = int(row[3]) #Supervisor total quota
    is_supervisor_free = row[4]
    is_area_free = row[4]
    area_quota = int(row[5]) #Quota by area
    for sp in supervisor: #Loop to create Distinct list of supervisors
        if(any(name in sp for sp in supervisor)): #If Supervisor exists
            break;
        if(any(ID not in st for st in student)): #If supervisor not found
            supervisor[k].append(name) #Add supervisor properties
            supervisor[k].append(supervisor_id)
            supervisor[k].append(quota)
            if(area_quota != 0):
                has_area_quota = True
            else:
                has_area_quota = False
            supervisor[k].append(bool(is_supervisor_free)) 
            supervisor[k].append(bool(has_area_quota))
    area[k].append(area_of_supervisor) #Add each area available
    area[k].append(area_quota)
    area[k].append(supervisor_id)
    area[k].append(bool(is_area_free))
    
    k += 1
supervisor = [x for x in supervisor if x!= []] #cleaning up the list




def get_name_of_preferred_area(stud): #Retrieve first preference of student.
    for i in range(0, len(student)):
        if student[i][0] == stud:
            return student[i][2]

def get_supervisor(ar): #Retrieving supervisor linked to area by FK
    for i in range(0, len(area)):
        if area[i][0] == ar:
            for j in range(0, len(supervisor)):
                if area[i][2] == supervisor[j][1]:
                    return supervisor[j][0]

def get_supervisor_quota(ar):
    for i in range(0, len(area)):
        if area[i][0] == ar:
            for j in range(0, len(supervisor)):
                if area[i][2] == supervisor[j][1]:
                    return supervisor[j][4]



def area_is_assigned(ar, sup): #check if quota has been met yet
    for i in range(0, len(area)):
        if area[i][0] == ar: #Find supervisor area
            for j in range(0, len(supervisor)):
                if supervisor[j][0] == sup: #Find supervisor area
                    if(supervisor[j][4]):
                        if(area[i][1] == 0): #If supervisor has quota by area, check if area is available
                            return True
                    if not(supervisor[j][4]):  
                        if(supervisor[j][2] == 0): #if supervisor has total quota only
                            return True #check if supervisor is available 
    return False

def reason_for_result(ar): ##For logging purposes
    has_ar_quota = get_supervisor_quota(ar)
    if not (has_ar_quota): #If Supervisor has no area quota then log supervisor unavailable
        audit.write("Reason for unavailability: Quota has been met for Supervisor %s\n" %(get_supervisor(ar)))
    if(has_ar_quota): #If Supervisor has area quota then log area unavailable
        audit.write("Reason for unavailability: Quota has been met for Area %s\n" %(ar))


def allocated_to(stud): #Check supervisor Availability
     for i in range(0, len(student)):
         if student[i][0] == stud: #new student gets their second preference and so on..
            audit.write("\n%s: Checking FYP availbility \n" %(student[i][1]))
            audit.write("Preference 1: %s with %s NOT AVAILABLE \n" %(student[i][2], get_supervisor(student[i][2])))
            reason_for_result(student[i][2]);
            for j in range(3, 7):
                if not area_is_assigned(student[i][j], get_supervisor(student[i][j])):
                    audit.write("Preference %s: %s with %s AVAILABLE \n" %(j-1, student[i][j], get_supervisor(student[i][j])))
                    student[i][j] = student[i][j]
                    return student[i][j]
                else:
                    audit.write("Preference %s: %s with %s NOT AVAILABLE \n" %(j-1, student[i][j], get_supervisor(student[i][j])))
                    reason_for_result(student[i][j]);

                

def assign(stud, ar, is_First): #Assigning supervisor to student
    if is_First:
        for i in range(0, len(student)):
            if student[i][0] == stud:
                audit.write("\n%s: Checking FYP availbility \n" %(student[i][1]))
                student[i][8] = ar
                audit.write("Preference 1: %s with %s AVAILABLE \n" %(student[i][2], get_supervisor(student[i][2])))
        for k in range(0, len(area)): #Marking the allocated area or supervisor as no longer free
            if area[k][0] == ar:
                for j in range(0,len(supervisor)):
                    if(area[k][2] == supervisor[j][1]):
                        if(supervisor[j][4]): #if supervisor has area quota
                            area[k][1] -= 1 #reduce area quota
                            if(area[k][1] == 0): #if the area quota is zero
                                area[k][3] = False #set the area to unavailable
                        if not(supervisor[j][4]): #if supervisor has total quota
                            supervisor[j][2] -= 1 #reduce total quota
                            if(supervisor[j][2] == 0): #if the total quota is zero
                                supervisor[j][3] = False #set the supervisor's availability to unavailable
    if not is_First: #For lower than first preference
       for i in range(0, len(student)):
            if student[i][0] == stud:
                student[i][8] = ar
       for k in range(0, len(area)): #Marking the allocated area or supervisor as no longer free
            if area[k][0] == ar:
                for j in range(0,len(supervisor)):
                    if(area[k][2] == supervisor[j][1]): ##Find supervisor linked to FK in area
                        if(supervisor[j][4]): #if supervisor has area quota
                            area[k][1] -= 1 #reduce area quota
                            if(area[k][1] == 0): #if the area quota is zero
                                area[k][3] = False #set the area to unavailable
                        if not(supervisor[j][4]): #if supervisor has total quota
                            supervisor[j][2] -= 1 #reduce total quota
                            if(supervisor[j][2] == 0): #if the total quota is zero
                                supervisor[j][3] = False #set the supervisor's availability to unavailable
  

def main():        
    for i in range(0, len(student)):
        stud = student[i][0] #Get student name
        #if not is_assigned(stud): #check if student is assigned
        areaforFYP = get_name_of_preferred_area(stud) #if not get first preference supervisor.
        supervisor_for_area = get_supervisor(areaforFYP)
        if area_is_assigned(areaforFYP, supervisor_for_area): #if their first preference is assigned:
            assignedArea = allocated_to(stud)#Check availabilities
            assign(stud, assignedArea, False) #Assign the student with the resultant area
        else:
            assign(stud, areaforFYP, True)#if the supervisor is still available, simply assign.


def end(): #Basic print implementation for test data
    print("Resolution:\n")
    for i in range(0, len(student)):
        stud = student[i][1]
        area = student[i][8] #add tutor name with final output
        sup = get_supervisor(area)

        print(stud + " is assigned to " + area + " with " + sup )
        returntoweb(stud, area, sup)
    

def returntoweb(stud, area, sup): #Returning allocation to Web App.
    writer.writerow([stud + " is assigned to " + area + " with " + sup])
    
        
   


main() #Execute methods main and end
end()
csv_test.close();
audit.close();
    
  



#Read arrays of data from csv file
#Execute Simulated SMP
#Write result to CSV file.

#To-do:
#1. Write resultant student and supervisor to csv file
#2. Read the csv result into a php file



