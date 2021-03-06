import tkinter as tk
import json
import requests
from bs4 import BeautifulSoup
import os
import xml.etree.ElementTree as ET


class Application(tk.Frame):

    def __init__(self, master=None):
        tk.Frame.__init__(self, master)
        self.patientInfo = {}
        self.formInfo = {}
        self.grid()
        self.initializeGUI()

    def initializeGUI(self):
        self.patientInfoFrame = tk.LabelFrame(
            self, text='Patient Info', padx=10, pady=10)
        self.patientInfoFrame.grid(row=0, column=0, sticky=tk.E+tk.W)
        self.populatePatientInfoFrame()

        self.formFrame = tk.LabelFrame(self, text='Form', padx=10, pady=10)
        self.formFrame.grid(row=1, column=0, sticky=tk.E+tk.W)
        self.populateFormFrame()

    def populatePatientInfoFrame(self):
        self.patientInfo['patientId'] = tk.StringVar(value='siimjoe')
        self.patientInfoFrame.pIdField = tk.Entry(
            master=self.patientInfoFrame, textvariable=self.patientInfo['patientId'])
        self.patientInfoFrame.pIdField.grid(row=0, column=0)

        self.patientInfoFrame.pullPatientInfoButton = tk.Button(
            master=self.patientInfoFrame, padx=10, pady=2, text="Pull Patient Info", command=self.pullPatientInfo)
        self.patientInfoFrame.pullPatientInfoButton.grid(row=0, column=1)

    def pullPatientInfo(self):
        print('Getting Patient Info')
        url = 'http://hackathon.siim.org/fhir/Patient/{}'.format(
            self.patientInfo['patientId'].get())
        heads = {
            'apikey': os.environ['SiimApiKey'],
            'Content-Type': 'application/fhir+json'
        }
        response = requests.get(url, headers=heads)
        self.patientInfo['data'] = response.text

    def populateFormFrame(self):
        self.formInfo['formID'] = tk.StringVar(value='50513')
        self.formFrame.formSelectField = tk.Entry(
            master=self.formFrame, textvariable=self.formInfo['formID'])
        self.formFrame.formSelectField.grid(row=0, column=0)

        self.formFrame.getFormButton = tk.Button(
            master=self.formFrame, text='Get Form', command=self.getForm, padx=10)
        self.formFrame.getFormButton.grid(row=0, column=1)

        self.formFrame.templateFrame = tk.Frame(
            master=self.formFrame, padx=10, pady=10)
        self.formFrame.templateFrame.grid(row=1, column=0, columnspan=3)

    def getForm(self):
        url = 'https://phpapi.rsna.org/radreport/v1/templates/{}/details'.format(
            self.formInfo['formID'].get())
        response = requests.get(url).text
        self.formInfo['data'] = json.loads(response)['DATA']['templateData']
        self.formInfo['html'] = BeautifulSoup(
            self.formInfo['data'], 'html.parser')
        # print(self.formInfo['html'])
        self.buildForm()

    def buildForm(self):
        print(self.formInfo['html'].title)
        for child in self.formFrame.templateFrame.winfo_children():
            child.destroy()
        self.formFrame.title = tk.Label(master=self.formFrame.templateFrame,
                                        anchor=tk.CENTER, padx=20, pady=5, text=self.formInfo['html'].title.text)
        self.formFrame.title.grid(row=2, column=1, columnspan=2)

        coding = self.formInfo['html']('script')[0].text
        self.formInfo['coding'] = coding.replace('<!--', '').replace('-->', '')
        self.formInfo['fields'] = {}

        # from flask import Flask
        # app = Flask(__name__)

        iRow = 3
        for section in self.formInfo['html']('section'):
            if ('findings') in section.attrs['id']:
                self.addTitleRow(self.formFrame.templateFrame,
                                 section.header.string, iRow)
                iRow += 1
                for row in section.find_all('p'):
                    self.addTextFieldRow(
                        self.formFrame.templateFrame, row.input['id'], row.label.string, row.input['value'], iRow)
                    iRow += 1

        self.formFrame.postButton = tk.Button(
            master=self.formFrame.templateFrame, text='Post', command=self.jsonDump, padx=10)
        self.formFrame.postButton.grid(row=iRow, column=1)

    def addTitleRow(self, parent, string, iRow):
        print('titles skipped for now')

    def addTextFieldRow(self, parent, id, name, defaultValue, iRow):
        # setattr(self.formFrame,id,tk.Entry())
        self.formInfo['fields'][id] = {
            'name': name, 'value': tk.StringVar(value=defaultValue)}
        setattr(self.formFrame, id+'Label', tk.Label(master=parent, text=name))
        getattr(self.formFrame, id+'Label').grid(row=iRow,
                                                 column=0, columnspan=2)
        setattr(self.formFrame, id+'entry', tk.Entry(master=parent,
                                                     width=50, textvariable=self.formInfo['fields'][id]['value']))
        getattr(self.formFrame, id+'entry').grid(row=iRow,
                                                 column=2, columnspan=3)

    def addDropDownRow(self, parent, id, name, iRow):
        print('drop down skipped for now')

    def jsonDump(self):
        self.patientInfo['dump'] = {'coding': self.formInfo['coding']}
        self.patientInfo['dump']['form'] = {}
        for field, values in self.formInfo['fields'].items():
            self.patientInfo['dump']['form'][field] = {
                'name': values['name'], 'value': values['value'].get()}
        print(json.dumps(self.patientInfo['dump']))


app = Application()
app.master.title('Synoptic Report Generator')
app.mainloop()
