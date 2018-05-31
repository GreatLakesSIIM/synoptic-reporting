import tkinter as tk
import requests
from bs4 import BeautifulSoup


class Application(tk.Frame):
    
    def __init__(self, master=None):
        tk.Frame.__init__(self, master)
        self.formID = tk.StringVar(value='235')
        self.grid()
        self.initializeForm()

    

    def initializeForm(self):
        self.formSelect = tk.Entry(self, textvariable=self.formID)
        self.formSelect.grid(column=0,row=0)
        self.quitButton = tk.Button(text='Get Form',command=self.get_form)
        self.quitButton.grid(column=1,row=0)


    def get_form(self):
        self.req = requests.get('http://radreport.org/html/htmldownload.php?id={}'.format(self.formID.get()))
        self.bs_form = BeautifulSoup(self.req.text,'html.parser')
        print(self.bs_form.prettify())


app = Application()
app.master.title('Sample application')
app.mainloop()

