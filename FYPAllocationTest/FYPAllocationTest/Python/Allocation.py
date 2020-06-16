import csv # used for reading csv files

# Setting global variables 
i = 0 # to iterate a 2D array for students
k = 0 # to iterate a 2D array for supervisors
y = 1 # to properly save student preference to the student array
pref = []
pref_supervisor = []
student = []
supervisor = []
area = []

# Setting streams reads and writes to be utilised
audit = open("wwwroot\\txt\\Allocation_Log.txt", 'w')
csv_file = open("wwwroot\\csv\\students.csv", 'r') # reading from csv file with student data
csv_result = open("wwwroot\\csv\\allocation_result.csv", 'w') # setting up csv file to contain result of allocation
supervisors_file = open("wwwroot\\csv\\supervisors.csv", 'r') # reading supervisor data from csv file
writer = csv.writer(csv_result, delimiter=",")
reader = csv.reader(csv_file)
sup_reader = csv.reader(supervisors_file)

for row in reader: # Each element within the csv file is read and saved below:
    student.append([]) #  Adding a new 2D element with each iteration
    name = row[1] # Saving row as student name
    ID = row[3] # Saving row as student Id
    pref = row[5] # Saving row as student preferene
    for st in student: # Checking if preference has been added to student
        if(any(ID in st for st in student)): # If preference is found, iterate to next pass
            student[i-y].append(pref)
            y+=1
            break;
        if(any(ID not in st for st in student)): # If preference has not yet been added
            y=1
            student[i].append(ID) # Add the preferene details to the array
            student[i].append(name)
            student[i].append(pref)
            break;
    i += 1

student = [x for x in student if x!= []] # Trim any possible whitespace that may form part of the array
for i in range(0, len(student)):
    student[i].append([]) # Adding empty field to be populated with assigned supervisor

for row in sup_reader: # Reading all rows from supervisor csv to create a supervisor and area array
    supervisor.append([]) # create new element for supervisor array
    area.append([]) # create new element for area array
    name = row[0] # supervisor name
    supervisor_id = row[1] # supervisor id
    area_of_supervisor = row[2] # area of research
    quota = int(row[3]) # Supervisor total quota
    is_supervisor_free = row[4] # check to see if supervisor is available
    is_area_free = row[4] # check to see if area is available
    area_quota = int(row[5]) # Quota by area
    area_ID = int(row[6]) # id for specific area
    for sp in supervisor: # Loop to create Distinct list of supervisors
        if(any(name in sp for sp in supervisor)): # If Supervisor exists
            break;
        if(any(ID not in sp for sp in supervisor)): # If supervisor not found
            supervisor[k].append(name) # Add supervisor properties
            supervisor[k].append(supervisor_id)
            supervisor[k].append(quota)
            if(area_quota != 0): # If supervisor has opted to have an area quota
                has_area_quota = True
            else: # If supervisor has no area quota
                has_area_quota = False
            supervisor[k].append(bool(is_supervisor_free)) # Add supervisor quota availability
            supervisor[k].append(bool(has_area_quota)) # Add area quota availability
    area[k].append(area_of_supervisor) # Add each area available
    area[k].append(area_quota) # Add area quota to array
    area[k].append(supervisor_id) # Add supervisor id to array
    area[k].append(bool(is_area_free)) # Add area availability to array
    area[k].append(area_ID) # Add area id to array
    k += 1
supervisor = [x for x in supervisor if x!= []] # cleaning up the list

def get_name_of_preferred_area(stud): # Retrieve first preference of student.
    for i in range(0, len(student)):
        if student[i][0] == stud:
            return student[i][2]

def get_supervisor(ar): # Retrieving supervisor linked to area by Foreign Key
    for i in range(0, len(area)):
        if area[i][0] == ar:
            for j in range(0, len(supervisor)):
                if area[i][2] == supervisor[j][1]:
                    return supervisor[j][0]
    return "Not assigned" # If student has not been assigned due to lack of availability

def supervisor_has_quota(ar): # Check whether or not supervisor has opted for an area quota or not
    for i in range(0, len(area)):
        if area[i][0] == ar:
            for j in range(0, len(supervisor)):
                if area[i][2] == supervisor[j][1]:
                    return supervisor[j][4]

def get_supervisor_quota(ar): # Retrieve supervisor total quota
    for i in range(0, len(area)):
        if area[i][0] == ar:
            for j in range(0, len(supervisor)):
                if area[i][2] == supervisor[j][1]:
                    return supervisor[j][2]

def get_supervisor_id(ar): # Retrieve ID bound to supervisor for area
    for i in range(0, len(area)):
        if area[i][0] == ar:
            for j in range(0, len(supervisor)):
                if area[i][2] == supervisor[j][1]:
                    return supervisor[j][1]

def get_area(stud): # Get the area title for a given area
    for i in range(0, len(student)):
        if student[i][1] == stud:
            return student[i][8]

def get_area_quota(ar): # Retrieve area quota
    for i in range(0, len(area)):
        if area[i][0] == ar:
            return area[i][1]

def get_area_id(ar): # Get area id for a given area
    for i in range(0, len(area)):
        if area[i][0] == ar:
            return area[i][4]
        

def area_is_assigned(ar, sup): # Check if quota has been met yet
    for i in range(0, len(area)):
        if area[i][0] == ar: # Find supervisor area
            for j in range(0, len(supervisor)):
                if supervisor[j][0] == sup: # Find supervisor area
                    if(supervisor[j][4]):
                        if(area[i][1] == 0): # If supervisor has quota by area, check if area is available
                            return True
                    if not(supervisor[j][4]):  
                        if(supervisor[j][2] == 0): # If supervisor has total quota only
                            return True # Check if supervisor is available 
    return False

def reason_for_result(ar): # For logging purposes
    has_ar_quota = supervisor_has_quota(ar)
    if not (has_ar_quota): # If Supervisor has no area quota, then log supervisor unavailable
        audit.write("Reason for unavailability: Quota has been met for Supervisor %s\n" %(get_supervisor(ar)))
    if(has_ar_quota): # If Supervisor has area quota, then log area unavailable
        audit.write("Reason for unavailability: Quota has been met for Area %s\n" %(ar))


def allocated_to(stud): # Check supervisor Availability
     for i in range(0, len(student)):
         if student[i][0] == stud: # Upon finding the required student
            audit.write("\n%s: Checking FYP availability \n" %(student[i][1]))
            audit.write("Preference 1: %s with %s NOT AVAILABLE \n" %(student[i][2], get_supervisor(student[i][2]))) # Log unavailable first preference
            reason_for_result(student[i][2]); # Log reason as to why first preference was not assigned
            for j in range(3, 9): # For each of the remaining five preferences
                if not area_is_assigned(student[i][j], get_supervisor(student[i][j])) and j != 8: # If area is available and within the top 6 preferences
                    audit.write("Preference %s: %s with %s AVAILABLE \n" %(j-1, student[i][j], get_supervisor(student[i][j]))) # Log allocation
                    student[i][j] = student[i][j] 
                    return student[i][j] # Return allocated result
                elif area_is_assigned(student[i][j], get_supervisor(student[i][j])) and j != 8: # If area is not available and other preferences remain to be checked
                    audit.write("Preference %s: %s with %s NOT AVAILABLE \n" %(j-1, student[i][j], get_supervisor(student[i][j]))) # Log lack of availability
                    reason_for_result(student[i][j]); # Provide details as to explain area not being allocated
                else:
                    audit.write("All Preference NOT AVAILABLE, please manually allocate \n") # If all preferences are taken, log the lack of availability

def assign(stud, ar, is_First): # Assigning supervisor to student
    if is_First: # If student is assigned to first preference
        for i in range(0, len(student)):
            if student[i][0] == stud: # Find the student by id
                audit.write("\n%s: Checking FYP availability \n" %(student[i][1]))
                student[i][8] = ar # Save assigned area to array
                audit.write("Preference 1: %s with %s AVAILABLE \n" %(student[i][2], get_supervisor(student[i][2]))) # Log allocation
        for k in range(0, len(area)): # Marking the allocated area or supervisor as no longer free
            if area[k][0] == ar:
                for j in range(0,len(supervisor)):
                    if(area[k][2] == supervisor[j][1]):
                        if(supervisor[j][4]): # If supervisor has area quota
                            area[k][1] -= 1 # Reduce area quota
                            if(area[k][1] == 0): # If the area quota is zero
                                area[k][3] = False # Set the area to unavailable
                        if not(supervisor[j][4]): # If supervisor has total quota
                            supervisor[j][2] -= 1 # Reduce total quota
                            if(supervisor[j][2] == 0): # If the total quota is zero
                                supervisor[j][3] = False # Set the supervisor's availability to unavailable          
    if not is_First: # If student is assigned to a preference other than first preference 
       for i in range(0, len(student)):
            if student[i][0] == stud:
                student[i][8] = ar # Save the assigned area to the student
       for k in range(0, len(area)): # Marking the allocated area or supervisor as no longer free
            if area[k][0] == ar:
                for j in range(0,len(supervisor)):
                    if(area[k][2] == supervisor[j][1]): # Find supervisor linked to Foreign Key in area
                        if(supervisor[j][4]): # If supervisor has area quota
                            area[k][1] -= 1 # Reduce area quota
                            if(area[k][1] == 0): # If the area quota is zero
                                area[k][3] = False # Set the area to unavailable
                        if not(supervisor[j][4]): # If supervisor has total quota
                            supervisor[j][2] -= 1 # Reduce total quota
                            if(supervisor[j][2] == 0): # If the total quota is zero
                                supervisor[j][3] = False # Set the supervisor's availability to unavailable
  
def main():        
    for i in range(0, len(student)):
        stud = student[i][0] # Get student name
        areaforFYP = get_name_of_preferred_area(stud) # get preferred area for student
        supervisor_for_area = get_supervisor(areaforFYP) # get preferred supervisor for student
        if area_is_assigned(areaforFYP, supervisor_for_area): # If first preference is assigned
            assignedArea = allocated_to(stud)# Check availabilities
            assign(stud, assignedArea, False) # If a preferred area or supervisor remains available, assign the student with the resultant area and supervisor
        else:
            assign(stud, areaforFYP, True)# If the supervisor and area are still available, immediately assign to student

def end(): # Basic print implementation for test data
    print("Allocation Result:\n")
    for i in range(0, len(student)):
        stud = student[i][1] # Set student name
        stud_id = student[i][0] # Set student id
        area = get_area(stud) # Add tutor name with final output
        area_id = get_area_id(area) # Set area id
        sup = get_supervisor(area) # Set supervisor name
        sup_id = get_supervisor_id(area) # Set supervisor id
        sup_quota = get_supervisor_quota(area) # Set supervisor quota
        area_quota = get_area_quota(area) # Set area quota
        if not area: # If student has remained unassigned, set the appropriate values to communicate the lack of assignment with the controller
            area = "No area"
            area_id = "0"
            sup_id = "None"
            print(stud + " is assigned to " + area + " having been " + sup )
            returntoweb(stud, stud_id, area, area_id, sup, sup_id, sup_quota, area_quota)
        else: # If student is successfully assigned
            print(stud + " is assigned to " + area + " with " + sup )
            returntoweb(stud, stud_id, area, area_id, sup, sup_id, sup_quota, area_quota)

def returntoweb(stud, stud_id, area, area_id, sup, sup_id, sup_quota, area_quota): # Returning allocation to the controller class through a csv file
    writer.writerow([stud , stud_id, area, area_id, sup, sup_id, sup_quota, area_quota ])
    
main() # Execute main method to corry out allocation
end() # Sort allocations and send back to controller class
csv_result.close(); # close the stream for 'csv_result'
audit.close(); # close the stream for 'audit'
    



